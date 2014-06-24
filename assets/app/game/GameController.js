'use strict';

cbApp.controller('GameController', ['$scope', '$log', 'SignalRGameService', '$rootScope',
    function SignalrController($scope, $log, SignalRGameService, $rootScope) {
        var self = this;
        $scope.revaledSquares = [];
        $scope.CreateGame = function() {
            SignalRGameService.CreateGame();
        };

        $scope.Range = function (n) {
            return new Array(n);
        };

        $scope.Click = function (square) {
            if (!square) return;
            SignalRGameService.ClickSquare(square.Row, square.Column);
        };

        $scope.Square = function(row, col) {
            if (!$scope.gameBoard) return {};
            var square = $scope.gameBoard.Squares.filter(function(data) {
                return data.Row == row && data.Column == col;
            });
            if (square.length != 1) return {};
            return square[0];
        };

        $scope.SquareStatus = function(square) {
            if (square.Status == undefined) return '';
            if (square.Status == 0) return '?';
            if (square.Bomb) return 'B';
            if (square.NeighboringBombs == 0) return '';
            return square.NeighboringBombs;
        }


        self.Init = function () {
            SignalRGameService.initialized.then(function() {
                SignalRGameService.JoinServer();
            });

            $rootScope.$on('game.gamePlayers', function (e, players) {
                $scope.gamePlayers = players;
                $scope.$apply();
            });

            $rootScope.$on('game.currentGames', function (e, games) {
                $scope.currentGames = games;
                $scope.$apply();
            });

            $rootScope.$on('game.joinServer', function () {
                SignalRGameService.CurrentGames();
            });
            
            $rootScope.$on('game.createGame', function (e, game) {
                $scope.currentGame = game;
                $scope.$apply();
            });
            
            $rootScope.$on('game.updateGameList', function (e, games) {
                $scope.currentGames = games;
                $scope.$apply();
            });

            $rootScope.$on('game.reveal', function (e, squares) {
                if (!squares || !angular.isArray(squares)) return;
                angular.forEach(squares, function(square) {
                    var boardSquare = $scope.gameBoard.Squares[square.Row][square.Column];

                    boardSquare.Bomb = square.Bomb;
                    boardSquare.NeighboringBombs = square.NeighboringBombs;
                    boardSquare.Status = square.Status;
                });
                $scope.$apply();
            });

            $rootScope.$on('game.initGameBoard', function (e, board) {
                $scope.gameBoard = board;
                $scope.$apply();
            });
        };
        self.Init();
    }]
);