'use strict';

cbApp.controller('SignalrController', ['$scope',
    function SignalrController($scope) {
        var self = this;

        $scope.CreateGame = function() {
            $scope.hub.server.createGame();
        };


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

            $.connection.hub.start().done(function() {
                $scope.hub.server.joinServer();
            });
        };
        self.Init();
    }]
);