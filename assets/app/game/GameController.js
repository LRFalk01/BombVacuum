'use strict';

cbApp.controller('GameController', ['$scope', '$log', 'SignalRGameService',
    function SignalrController($scope, $log, SignalRGameService) {
        var self = this;
        $scope.SignalRGameService = SignalRGameService;
        $scope.revaledSquares = [];
        $scope.CreateGame = function() {
            SignalRGameService.CreateGame();
        };

        $scope.Range = function (n) {
            return new Array(n);
        };

        self.Init = function () {
            SignalRGameService.initialized.then(function() {
                SignalRGameService.CurrentGames();
            });
        };
        self.Init();
    }]
);