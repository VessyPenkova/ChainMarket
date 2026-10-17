
using System.Collections.Generic;
using ChainMarket.Web.Models;

namespace ChainMarket.Web.Data
{
    public static class StandardChainNomenclature
    {
        public static List<ChainNomenclature> GetItems()
        {
            var items = new List<ChainNomenclature>();

            string[] rollerSizes =
            {
                "06B", "08B", "10B",
                "12B", "16B", "20B"
            };

            // 1. РОЛКОВИ ВЕРИГИ
            // Едноредови, двуредови, триредови
            foreach (string size in rollerSizes)
            {
                for (int strands = 1; strands <= 3; strands++)
                {
                    items.Add(new ChainNomenclature
                    {
                        Code = $"{size}-{strands}",
                        CategoryId = 1,
                        Description = strands switch
                        {
                            1 => "Едноредова ролкова верига",
                            2 => "Двуредова ролкова верига",
                            _ => "Триредова ролкова верига"
                        }
                    });
                }
            }

            // 2. ТРАНСПОРТЬОРНИ ВЕРИГИ
            // Примери с A1 закрепващи планки.
            // Точните варианти се проверяват по каталог.
            string[] conveyorCodes =
            {
                "08B-1-A1",
                "10B-1-A1",
                "12B-1-A1"
            };

            foreach (string code in conveyorCodes)
            {
                items.Add(new ChainNomenclature
                {
                    Code = code,
                    CategoryId = 2,
                    Description = "Верига с закрепващи планки A1"
                });
            }

            // 3. ПОДЕМНИ ВЕРИГИ
            // Серия BL - листови вериги
            string[] liftingCodes =
            {
                "BL423", "BL444",
                "BL523", "BL544"
            };

            foreach (string code in liftingCodes)
            {
                items.Add(new ChainNomenclature
                {
                    Code = code,
                    CategoryId = 3,
                    Description = "Листова подемна верига"
                });
            }

            // 4. СЕЛСКОСТОПАНСКИ ВЕРИГИ
            string[] agriculturalCodes =
            {
                "CA550", "CA555", "CA620"
            };

            foreach (string code in agriculturalCodes)
            {
                items.Add(new ChainNomenclature
                {
                    Code = code,
                    CategoryId = 4,
                    Description = "Селскостопанска верига"
                });
            }

            // 5. НЕРЪЖДАЕМИ И СПЕЦИАЛНИ ВЕРИГИ
            // SS е вътрешно означение за материалния вариант.
            foreach (string size in rollerSizes)
            {
                for (int strands = 1; strands <= 3; strands++)
                {
                    items.Add(new ChainNomenclature
                    {
                        Code = $"{size}-{strands}-SS",
                        CategoryId = 5,
                        Description =
                            "Неръждаема ролкова верига - " +
                            strands + " ред(а)"
                    });
                }
            }

            // 6. СВЪРЗВАЩИ ЗВЕНА И АКСЕСОАРИ
            // CL = Connecting Link
            // OL = Offset Link
            // Това са вътрешни кодове на ChainMarket.
            foreach (string size in rollerSizes)
            {
                for (int strands = 1; strands <= 3; strands++)
                {
                    string chainCode = $"{size}-{strands}";

                    items.Add(new ChainNomenclature
                    {
                        Code = $"{chainCode}-CL",
                        CategoryId = 6,
                        Description =
                            $"Свързващо звено за {chainCode}"
                    });

                    items.Add(new ChainNomenclature
                    {
                        Code = $"{chainCode}-OL",
                        CategoryId = 6,
                        Description =
                            $"Криво (преходно) звено за {chainCode}"
                    });
                }
            }

            return items;
        }
    }
}
