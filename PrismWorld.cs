using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

using prismmod.NPCs.WaterTown;
using prismmod.Tiles.Blox;
using prismmod.Walls;
using static prismmod.PrismHelper;

namespace prismmod
{
    internal class PrismWorld : ModWorld
    {
        public static int moistChiseledStoneCount = 0;
        public bool killedGargantuanTortoise;
        public bool downedPrismachine;

        public override void Initialize()
        {
            killedGargantuanTortoise = false;
            downedPrismachine = false;
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = killedGargantuanTortoise;
            flags[1] = downedPrismachine;
            writer.Write(flags);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            killedGargantuanTortoise = flags[0];
            downedPrismachine = flags[1];
        }

        //@todo figure out what the hell the Save and Load functions do
        //@body what even is a TagCompound?
        //save/load functions here

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            /*int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (genIndex != -1)
            {
                tasks.Insert(genIndex + 1, new PassLegacy("Reserved Prismmod Test Space", delegate (GenerationProgress progress)
                {
                    progress.Message = "Test Code Here ;)";
                }));
            }*/

            //@todo fix infinite glass spawning before Smoothing World gen task
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (genIndex != -1)
            {
                tasks.Insert(genIndex + 1, new PassLegacy("Generate  Water Town", delegate (GenerationProgress progress)
                {
                    //@todo make placeholder for gate block
                    //@body create class and image for block that acts as a gate to the biome
                    //@todo add gate for water town
                    //@body use glass tunnel coating to obtain x and y coords for gate placement to keep out the ne'er do wells
                    bool placedGate = false;//used to check if gateX and gateY should be set
                    int gateY;
                    int gateX;
                    progress.Message = "Tunneling";

                    //@todo update activeBlock to unbreakable block
                    //@body force braden to make another placeholder unbreakable block for guarding the biome
                    int activeBlock = ModContent.TileType<CityWall>();//TileID.Glass;

                    for (int xCoord = 59; xCoord < 72; xCoord++)
                    {
                        for (int yCoord = Main.spawnTileY - 70; yCoord < Main.spawnTileY + 120; yCoord++)
                        {
                            Tile tile = Framing.GetTileSafely(xCoord, yCoord);
                            tile.ClearTile();
                            if ((xCoord == 59 || xCoord == 71) 
                            && ((Framing.GetTileSafely(58, yCoord).liquid <= 2 && Framing.GetTileSafely(58, yCoord).active()) 
                            || (Framing.GetTileSafely(72, yCoord).liquid <= 2 && Framing.GetTileSafely(72, yCoord).active())) 
                            || (Framing.GetTileSafely(xCoord,yCoord-1).type == TileID.Glass))
                            {
                                if(!placedGate)
                                {
                                    placedGate=true;
                                    gateY = yCoord;
                                    gateX = xCoord;
                                }
                                WorldGen.PlaceTile(xCoord, yCoord, activeBlock);
                            }
                        }
                    }

                    //Framing.GetTileSafely(gateX, gateY);

                    for (int xCoord = 59; xCoord < 210; xCoord++)
                    {
                        for (int yCoord = Main.spawnTileY + 120; yCoord < Main.spawnTileY + 280; yCoord++)
                        {
                            Tile tile = Framing.GetTileSafely(xCoord, yCoord);
                            tile.ClearTile();
                            if (((xCoord == 59 || xCoord == 209) || (yCoord == Main.spawnTileY + 279 || yCoord == Main.spawnTileY + 120)) && !(yCoord == Main.spawnTileY + 120 && ((xCoord - 59) < 12) && (xCoord - 59) > 0))
                            {
                                WorldGen.PlaceTile(xCoord, yCoord, activeBlock);
                            }
                            else 
                            {
                                tile.liquid = 255;
                                tile.liquidType(0);
                                tile.liquid = 255;
                            }
                        }
                    }

                    activeBlock = ModContent.TileType<Tiles.Blox.MoistChiseledStone>();

                    int x = 80;
                    int y = (int)Main.spawnTileY + 135;
                    progress.Message = "Making the Cube";
                    for (int i = 0; i < 10; i++)
                    {
                        WorldGen.PlaceTile(x + i + 1, y, activeBlock);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        WorldGen.PlaceTile(x, y + i, activeBlock);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        WorldGen.PlaceTile(x + 10, y + i, activeBlock);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        WorldGen.PlaceTile(x + i, y + 10, activeBlock);
                    }

                    //@todo Add fish npcs
                    progress.Message = "Importing Fish People";
                    NPC.NewNPC((100)*16, (Main.spawnTileY + 130)*16,ModContent.NPCType<FishBlue>());

                }));
            }
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            moistChiseledStoneCount = tileCounts[ModContent.TileType<Tiles.Blox.MoistChiseledStone>()]; //update with custom mod block
        }
    }
}