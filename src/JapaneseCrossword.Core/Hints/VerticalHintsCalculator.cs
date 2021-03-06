﻿using System.Collections.Generic;
using General;
using JapaneseCrossword.Core.Extensions;
using JapaneseCrossword.Core.Rules;

namespace JapaneseCrossword.Core.Hints
{
    public class VerticalHintsCalculator:HintsCalculator
    {
        public VerticalHintsCalculator(MonochromeCell[,] cellData, IConsequitiveElementsCountFinder consequitiveElementsCountFinder) : base(cellData, consequitiveElementsCountFinder)
        {
        }

        public override int[,] Calculate()
        {
            var hints = GetConsecuitiveVerticalElements();
            return ComplexColectionHelpers.ListArrayToJaggedArrayVertical(hints);
        }

        public List<int>[] GetConsecuitiveVerticalElements()
        {
            var dimensionLength = CellData.GetLength(1);
            var hintsPerCol = new List<int>[dimensionLength];
            for (var col = 0; col < dimensionLength; col++)
            {
                var columnElements = CellData.GetColElements(col);
                var counts = ConsequitiveElementsFinder.Find(columnElements);
                hintsPerCol[col] = counts;
            }

            return hintsPerCol;
        }
    }
}
