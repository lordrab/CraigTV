var app = angular.module("superBob", ['ui.bootstrap'])

app.filter("trustAsHtml", ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    }
}]);