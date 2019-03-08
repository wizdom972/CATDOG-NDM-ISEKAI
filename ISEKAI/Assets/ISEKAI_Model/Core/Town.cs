using System;

namespace ISEKAI_Model
{
    public class Town
    {
        public Town() // initiallizes town instance with basic stats.
        {
            remainFoodAmount = 50f;
            maxFoodConsumption = 100f;
            totalFoodProduction = 10000000f;
            totalPleasantAmount = 100f;
            pleasantWeightFactor = 1f;
            suggestedFoodConsumption = 50f;
            totalIronProduction = 0f;
            totalHorseAmount = 0f;
            totalHorseProduction = 0f;
            totalIronAmount = 0f;
        }
        public float remainFoodAmount {get; set;}
        public float totalFoodProduction {get; set;}
        public float maxFoodConsumption {get; set;}
        public float totalFoodConsumption => Math.Min(maxFoodConsumption, remainFoodAmount / 2);
        public float totalPleasantAmount {get; set;}
        public float pleasantWeightFactor {get; set;}
        public float suggestedFoodConsumption {get; set;}
        public float pleasantChange => pleasantWeightFactor * (totalFoodConsumption - suggestedFoodConsumption) + pleasantChangeAddition;
        public float pleasantChangeAddition = 0;
        public float totalIronAmount { get; set; }
        public float totalIronProduction {get; set;}
        public float totalHorseAmount {get; set;}
        public float totalHorseProduction { get; set; }
        
        public void AddFoodProduction() // just adds current food production to remaining food amount.
        {
            remainFoodAmount += totalFoodProduction;
        }

        public void AddFoodProcuction(float toAdd) // adds value to remaining food amount.
        {
            if (toAdd < 0)
                throw new InvalidOperationException("You can't add negative value. Try using ConsumeFood().");
            remainFoodAmount += toAdd;
        }
        public void ConsumeFood() // just subtracts current food consumptions from remaining food amount.
        {
            remainFoodAmount -= totalFoodConsumption;
        }
        public void ConsumeFood(float toConsume) // subtracts value from remaining food amount.
        {
            if (toConsume < 0)
                throw new InvalidOperationException("You can't consume negative value. Try using AddFoodProduction().");
            remainFoodAmount -= toConsume;
        }

        public void ApplyPreTurnChange()
        {
            AddFoodProduction();
        }

        public void ApplyPostTurnChange()
        {
            totalPleasantAmount = Math.Max(0, Math.Min(200, totalPleasantAmount + pleasantChange));
            totalIronAmount += totalIronProduction;
            totalHorseAmount += totalHorseProduction;
            ConsumeFood();
        }
    }
}