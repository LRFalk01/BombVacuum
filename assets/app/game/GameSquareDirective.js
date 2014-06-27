'use strict';

cbApp.directive('bvGameSquare', ['SignalRGameService', function (SignalRGameService) {
    return {
        restrict: 'A',
        required: 'ngModel',
        scope: {
            ngModel: '='
        },
        templateUrl: 'assets/app/game/GameSquare.html',
        link: function(scope, element, attrs, ngModelCtrl) {
            scope.SquareState = function() {
                if (scope.ngModel.State == undefined) return '';
                if (scope.ngModel.State == 0) return '?';
                if (scope.ngModel.Bomb) return 'B';
                if (scope.ngModel.NeighboringBombs == 0) return '';
                return scope.ngModel.NeighboringBombs;
            };

            scope.Click = function() {
                SignalRGameService.ClickSquare(scope.ngModel.Row, scope.ngModel.Column);
            };

            element.bind('contextmenu', function(event) {
                event.preventDefault();
                event.stopPropagation();
                //todo flag square
            });
        }
    }
}]);
