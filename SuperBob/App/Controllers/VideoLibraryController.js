
app.controller("videoLibraryCtrl", function($scope,$http) {

    $scope.superListData = {
        displayUrl: '/VideoLibrary/DisplayList',
        addEditUrl: '/VideoLibrary/EditData',
        saveDataUrl: '/VideoLibrary/SaveCurrentData'
    }

})