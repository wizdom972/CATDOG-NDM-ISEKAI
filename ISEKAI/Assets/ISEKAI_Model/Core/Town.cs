using System;

namespace ISEKAI_Model
{
    public class Town
    {
        public Town() // initiallizes town instance with basic stats.
        {
            remainFoodAmount = 100000f;
            maxFoodConsumption = 100f;
            totalFoodProduction = 50f;
            totalPleasantAmount = 100f;
            pleasantWeightFactor = 1f;
            suggestedFoodConsumption = 80f;
            totalIronProduction = 0f;
            totalHorseAmount = 0f;
        }
        public float remainFoodAmount {get; set;}
        public float totalFoodProduction {get; set;}
        public float maxFoodConsumption {get; set;}
        public float totalFoodConsumption => Math.Max(maxFoodConsumption, remainFoodAmount / 2);
        public float totalPleasantAmount {get; set;}
        public float pleasantWeightFactor {get; set;}
        public float suggestedFoodConsumption {get; set;}
        public float pleasantChange => pleasantWeightFactor * (totalFoodConsumption - suggestedFoodConsumption);
        public float totalIronProduction {get; set;}
        public float totalHorseAmount {get; set;}
        
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
        public void ApplyPleasantChange() // apply current pleasant change to total pleasant amount.
        {
            totalPleasantAmount = Math.Min(200, totalPleasantAmount + pleasantChange);
        }
    }
}