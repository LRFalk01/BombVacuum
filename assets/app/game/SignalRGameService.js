'use strict';

cbApp.factory('SignalRGameService', ['$q', '$rootScope', function ($q, $rootScope) {
    var self = this;

    self.startDeferred = $q.defer();
    self.startPromise = self.startDeferred.promise;

    self.proxy = null;
  
    self.Init = function () {
        //Getting the connection object
        var connection = $.hubConnection();
  
        //Creating proxy
        self.proxy = connection.createHubProxy('gameHub');
  
        //Starting connection
        self.proxy.connection.start().then(function () {
            self.startDeferred.resolve();
        });

        self.proxy.on('gamePlayers', function (players) {
            $rootScope.$emit("game.gameplayers", players);
        });

        self.proxy.on('currentGames', function (games) {
            $rootScope.$emit("game.currentGames", games);
        });

        self.proxy.on('joinServer', function () {
            $rootScope.$emit("game.joinServer");
        });

        self.proxy.on('createGame', function (game) {
            $rootScope.$emit("game.createGame", game);
        });

        self.proxy.on('updateGameList', function (games) {
            $rootScope.$emit("game.updateGameList", games);
        });

        self.proxy.on('reveal', function (squares) {
            $rootScope.$emit("game.reveal", squares);
        });

        self.proxy.on('initGameBoard', function (board) {
            $rootScope.$emit("game.initGameBoard", board);
        });
    };
  
  
    self.CurrentGames = function () {
        self.proxy.invoke('currentGames');
    };
    self.JoinServer = function () {
        self.proxy.invoke('joinServer');
    };
    self.ClickSquare = function (row, col) {
        self.proxy.invoke('click', row, col);
    };

    self.CreateGame = function () {
        self.proxy.invoke('createGame');
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