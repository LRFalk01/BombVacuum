'use strict';

cbApp.controller('GameController', ['$scope', '$log',
    function SignalrController($scope, $log) {
        var self = this;
        $scope.revaledSquares = [];
        $scope.CreateGame = function() {
            $scope.hub.server.createGame();
        };

        $scope.Range = function (n) {
            return new Array(n);
        };

        $scope.Click = function (square) {
            if (!square) return;
            $scope.hub.server.click(square.Row, square.Column);
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
            $scope.hub = $.connection.gameHub;
            $scope.hub.client.gamePlayers = function (players) {
                $scope.gamePlayers = players;
                $scope.$apply();
            };

            $scope.hub.client.currentGames = function (games) {
                $scope.currentGames = games;
                $scope.$apply();
            };

            $scope.hub.client.joinServer = function () {
                $scope.hub.server.currentGames();
            };
            
            $scope.hub.client.createGame = function (game) {
                $scope.currentGame = game;
                $scope.$apply();
            };
            
            $scope.hub.client.updateGameList = function (games) {
                $scope.currentGames = games;
                $scope.$apply();
            };

            $scope.hub.client.reveal = function (squares) {
                if (!squares || !angular.isArray(squares)) return;
                angular.forEach(squares, function(square) {
                    var boardSquare = $scope.gameBoard.Squares[square.Row][square.Column];

                    boardSquare.Bomb = square.Bomb;
                    boardSquare.NeighboringBombs = square.NeighboringBombs;
                    boardSquare.Status = square.Status;
                });
                $scope.$apply();
            };

            $scope.hub.client.initGameBoard = function (board) {
                $scope.gameBoard = board;
                $scope.$apply();
            };

            $.connection.hub.start().done(function() {
                $scope.hub.server.joinServer();
            });
        };
        self.Init();
    }]
);