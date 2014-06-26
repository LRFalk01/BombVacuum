'use strict';

cbApp.controller('GameController', ['$scope', '$log', 'SignalRGameService', '$rootScope', '$timeout',
    function SignalrController($scope, $log, SignalRGameService, $rootScope, $timeout) {
        var self = this;
        $scope.revaledSquares = [];
        $scope.CreateGame = function() {
            SignalRGameService.CreateGame();
        };

        $scope.Range = function (n) {
            return new Array(n);
        };

        self.Init = function () {
            $rootScope.$on('game.gamePlayers', function (e, players) {
                $timeout(function () {
                    $scope.gamePlayers = players;
                    $scope.$apply();
                });
            });

            $rootScope.$on('game.currentGames', function (e, games) {
                $timeout(function () {
                    $scope.currentGames = games;
                    $scope.$apply();
                });
            });

            $rootScope.$on('game.joinServer', function () {
                $timeout(function () {
                    SignalRGameService.CurrentGames();
                });
            });
            
            $rootScope.$on('game.createGame', function (e, game) {
                $timeout(function () {
                    $scope.currentGame = game;
                    $scope.$apply();
                });
            });
            
            $rootScope.$on('game.updateGameList', function (e, games) {
                $timeout(function () {
                    $scope.currentGames = games;
                    $scope.$apply();
                });
            });

            $rootScope.$on('game.reveal', function (e, squares) {
                $timeout(function () {
                    if (!squares || !angular.isArray(squares)) return;
                    angular.forEach(squares, function (square) {
                        var boardSquare = $scope.gameBoard.Squares[square.Row][square.Column];

                        boardSquare.Bomb = square.Bomb;
                        boardSquare.NeighboringBombs = square.NeighboringBombs;
                        boardSquare.Status = square.Status;
                    });
                    $scope.$apply();
                });
            });

            $rootScope.$on('game.initGameBoard', function (e, board) {
                $timeout(function () {
                    $scope.gameBoard = board;
                    $scope.$apply();
                });
            });
            
            SignalRGameService.initialized.then(function () {
                $timeout(function() {
                    SignalRGameService.JoinServer();
                });
            });
        };
        self.Init();
    }]
);