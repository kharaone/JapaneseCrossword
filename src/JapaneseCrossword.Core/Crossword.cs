﻿using System;
using System.Collections.Generic;
using General;
using JapaneseCrossword.Core.Hints;
using JapaneseCrossword.Core.Rules;
using JapaneseCrossword.Core.State;

namespace JapaneseCrossword.Core
{
    public class Crossword
    {
        private GameProgress _progress;
        public IMonochrome<ColorChangedEventArgs>[,] Current => _progress.Current;
        private readonly IRules _rules;
        private readonly IStateLoader _stateLoader;
        private readonly IMainGridBuilder _mainGridBuilder;
        private readonly List<IHintsGridBuider> _hintsGridBuilders;
        private IHintsCalculator _verticalHintsCalculator;
        private IHintsCalculator _horizontalHintsCalculator;
        private IAnnouncer _announcer;

        public Crossword(MonochromeCell[,] gridData, IRules rules, IStateLoader loader,
            IMainGridBuilder mainGridBuilder, List<IHintsGridBuider> hintsBuilders, IAnnouncer announcer)
        {
            _rules = rules;
            _stateLoader = loader;
            _mainGridBuilder = mainGridBuilder;
            _announcer = announcer;
            ReloadHints(gridData);
            _hintsGridBuilders = hintsBuilders;
            BuildGame(gridData);
        }

        public void ReloadHints(MonochromeCell[,] gridData)
        {
            _verticalHintsCalculator = new VerticalHintsCalculator(gridData, new ConsequitiveElementsFinder());
            _horizontalHintsCalculator = new HorizontalHintsCalculator(gridData, new ConsequitiveElementsFinder());
        }

        private void BuildGame(MonochromeCell[,] gridData)
        {
            _progress = new GameProgress(gridData);
            _mainGridBuilder.Build(gridData);
            var horizontalHintsGridData = _verticalHintsCalculator.Calculate().InvertOrientation();
            var verticalHintsGridData = _horizontalHintsCalculator.Calculate().InvertOrientation();
            foreach (var hintsGridBuilder in _hintsGridBuilders)
            {
                var hintsData = hintsGridBuilder.IsVertical ? verticalHintsGridData : horizontalHintsGridData;
                hintsGridBuilder.Build(hintsData);
            }

            BindTiles();
        }

        private void BindTiles()
        {
            foreach (var tile in Current)
            {
                tile.ColorChanged += (sender, args) => OnCrosswordTileColorChange();
            }
        }

        private void OnCrosswordTileColorChange()
        {
            var isDone = IsGameOver();
            if (isDone)
            {
                _announcer.Show("Congratulations! You completed the crossword!");
            }
        }

    public bool IsGameOver()
        {
            return _rules.IsComplete(_progress);
        }

        public void Load(string path)
        {
            Clean();
            var loadedProgress = _stateLoader.Load(path);
            ReloadHints(loadedProgress.Goal);
            BuildGame(loadedProgress);
        }

        private void BuildGame(GameProgress progress)
        {
            BuildGame(progress.Goal);
            _mainGridBuilder.Reveal(progress.Current);
            _progress = progress;
        }

        public void Clean()
        {
            _mainGridBuilder.Clean();
            foreach (var hintBuilder in _hintsGridBuilders)
            {
                hintBuilder.Clean();
            }
        }

        public void Save(string path)
        {
            _stateLoader.Save(_progress, path);
        }

        public void SaveCustom(string path)
        {
            var progressCustom = new GameProgress(_progress.Current);
            _stateLoader.Save(progressCustom, path);
        }

        public void BuildMainGrid(int cols, int rows)
        {
            _mainGridBuilder.Build(cols, rows);
        }

        private void BuildHintGrids()
        {
            var horizontalHintsGridData = _verticalHintsCalculator.Calculate().InvertOrientation();
            var verticalHintsGridData = _horizontalHintsCalculator.Calculate().InvertOrientation();

            foreach (var hintsGridView in _hintsGridBuilders)
            {
                var hintsData = hintsGridView.IsVertical ? verticalHintsGridData : horizontalHintsGridData;
                hintsGridView.Build(hintsData);
            }
        }

        public void InvertCell(int row, int col)
        {
            _progress.InvertCell(row, col);
        }

        public void Initialise()
        {
            BuildMainGrid(_progress.Goal.GetLength(0), _progress.Goal.GetLength(1));
            BuildHintGrids();
        }

        public void Reveal()
        {
            _mainGridBuilder.Reveal(_progress.Goal);
        }

        public void BackToProgress()
        {
            _mainGridBuilder.Reveal(_progress.Current);
        }
    }
}
