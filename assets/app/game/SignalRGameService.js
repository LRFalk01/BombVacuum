'use strict';

cbApp.factory('SignalRGameService', ['$q', '$rootScope', '$timeout', '$log', function ($q, $rootScope, $timeout, $log) {
    var self = this;

    self.startDeferred = $q.defer();
    self.startPromise = self.startDeferred.promise;

    self.proxy = null;
    self.hub = null;
  
    self.Init = function () {
        //Getting the connection object
        var connection = $.hubConnection();
        self.hub = $.connection.gameHub;
  
        //Creating proxy
        self.proxy = connection.createHubProxy('gameHub');
  
        //Starting connection
        $.connection.hub.start().done(function () {
            $timeout(function () {
                self.startDeferred.resolve();
                $log.debug('connected');
            });
        });

        self.hub.client.gamePlayers = function (players) {
            $timeout(function () {
                $rootScope.$emit("game.gameplayers", players);
                $log.debug('game.gameplayers');
            });
        };

        self.hub.client.currentGames = function (games) {
            $timeout(function () {
                $rootScope.$emit("game.currentGames", games);
                $log.debug('game.currentGames');
            });
        };

        self.hub.client.joinServer = function () {
            $timeout(function () {
                $rootScope.$emit("game.joinServer");
                $log.debug('game.joinServer');
            });
        };

        self.hub.client.createGame = function (game) {
            $timeout(function () {
                $rootScope.$emit("game.createGame", game);
                $log.debug('game.createGame');
            });
        };

        self.hub.client.updateGameList = function (games) {
            $timeout(function () {
                $rootScope.$emit("game.updateGameList", games);
                $log.debug('game.updateGameList');
            });
        };

        self.hub.client.reveal = function (squares) {
            $timeout(function () {
                $rootScope.$emit("game.reveal", squares);
                $log.debug('game.reveal');
            });
        };

        self.hub.client.initGameBoard = function (board) {
            $timeout(function () {
                $rootScope.$emit("game.initGameBoard", board);
                $log.debug('game.initGameBoard');
            });
        };
    };
  
  
    self.CurrentGames = function () {
        $timeout(function () {
            self.hub.server.currentGames();
            $log.debug('currentGames');
        });
    };
    self.JoinServer = function () {
        $timeout(function () {
            self.hub.server.joinServer();
            $log.debug('joinServer');
        });
    };
    self.ClickSquare = function (row, col) {
        $timeout(function () {
            self.hub.server.click(row, col);
            $log.debug('click');
        });
    };

    self.CreateGame = function () {
        $timeout(function () {
            self.hub.server.createGame();
            $log.debug('createGame');
        });
    };

    self.Init();
    return {
        initialized: self.startPromise,
        CurrentGames: self.CurrentGames,
        JoinServer: self.JoinServer,
        ClickSquare: self.ClickSquare,
        CreateGame: self.CreateGame
    }; 
}]);