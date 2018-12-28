﻿using System.Windows.Input;
using JapaneseCrossword.Core;

namespace JapaneseCrossword.DesktopClient.ViewModel
{
    internal class BuildGameRandomCommand:BaseCommand, ICommand
    {
        private readonly GameViewModel _viewModel;

        public BuildGameRandomCommand(GameModel model, GameViewModel viewModel) : base(model)
        {
            _viewModel = viewModel;
        }

        public void Execute(object parameter)
        {

            var gridSize = model.ParseGridSize();
            if (gridSize == null)
            {
                return;
            }

            var dataGenerator = new GridDataGenerator();
            var cols = gridSize.Item1;
            var rows = gridSize.Item2;
            var gridData = dataGenerator.Generate(cols, rows);
            _viewModel.BuildGame(gridData);
        }

    }
}
