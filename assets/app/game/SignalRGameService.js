'use strict';

cbApp.factory('SignalRGameService', ['$q', '$rootScope', '$log', function ($q, $rootScope, $log) {
    var self = this;
    self.base = {};
    
    self.startDeferred = $q.defer();
    self.base.initialized = self.startDeferred.promise;
    self.base.currentGames = [];
    self.base.currentGame = {};
    self.base.gameBoard = {};

    self.base.gamePlayers = [];

    self.hub = null;
  
    self.Init = function () {
        self.hub = $.connection.gameHub;
  
        self.hub.client.gamePlayers = function (players) {
            angular.copy(players, self.base.gamePlayers);
            $rootScope.$apply();
            $log.debug('game.gameplayers');
        };

        self.hub.client.currentGames = function (games) {
            angular.copy(games, self.base.currentGames);
            $rootScope.$apply();
            $log.debug('game.currentGames');
        };

        self.hub.client.joinServer = function () {
            $log.debug('game.joinServer');
        };

        self.hub.client.createGame = function (game) {
            angular.copy(game, self.base.currentGame);
            $rootScope.$apply();
            $log.debug('game.createGame');
        };

        self.hub.client.updateGameList = function (games) {
            angular.copy(games, self.base.currentGames);
            $rootScope.$apply();
                $log.debug('game.updateGameList');
        };

        self.hub.client.reveal = function (squares) {
            if (!squares || !angular.isArray(squares)) return;
            angular.forEach(squares, function (square) {
                var boardSquare = self.base.gameBoard.Squares[square.Row][square.Column];

                boardSquare.Bomb = square.Bomb;
                boardSquare.NeighboringBombs = square.NeighboringBombs;
                boardSquare.State = square.State;
            });
            $rootScope.$apply();
            $log.debug('game.reveal');
        };

        self.hub.client.flag = function (square) {
            if (!square || !angular.isObject(square)) return;
            var boardSquare = self.base.gameBoard.Squares[square.Row][square.Column];

            boardSquare.Flag = square.Flag;
            $rootScope.$apply();
            $log.debug('game.flag');
        };

        self.hub.client.initGameBoard = function (board) {
            angular.copy(board, self.base.gameBoard);
            $rootScope.$apply();
            $log.debug('game.initGameBoard');
        };

        //Starting connection
        $.connection.hub.start().done(function () {
            self.startDeferred.resolve();
            //self.JoinServer();
            $rootScope.$apply();
            $log.debug('connected');
        });
    };
  
    self.CurrentGames = function () {
        self.hub.server.currentGames();
        $log.debug('currentGames');
    };
    self.JoinServer = function () {
        self.hub.server.joinServer();
        $log.debug('joinServer');
    };
    self.ClickSquare = function (row, col) {
        self.hub.server.click(row, col);
        $log.debug('click');
    };
    self.FlagSquare = function (row, col) {
        self.hub.server.flag(row, col);
        $log.debug('flag');
    };

    self.CreateGame = function () {
        self.hub.server.createGame();
        $log.debug('createGame');
    };

    self.JoinGame = function (gameId) {
        self.hub.server.joinGame(gameId);
        $log.debug('joinGame');
    };



    self.Init();
    return {
        initialized: self.base.initialized,

        currentGames: self.base.currentGames,
        currentGame: self.base.currentGame,
        gamePlayers: self.base.gamePlayers,
        gameBoard: self.base.gameBoard,

        CurrentGames: self.CurrentGames,
        ClickSquare: self.ClickSquare,
        FlagSquare: self.FlagSquare,
        CreateGame: self.CreateGame,
        JoinGame: self.JoinGame
    }; 
}]);