
app.controller("videoPlayerCtrl", function ($scope, $http, $uibModal) {

    $scope.playListModel = { Name: '', Id: 0 };

    $scope.dataSave = '';

    $scope.addVideoHide = { masterPlayList: true, playListName: false, playListVideos: false }
  
    $scope.addVideoList = function (index, rowIndex, saveData,getData) {
        $scope.dataSave = saveData;
        
        if (index != 0) {
            console.log(getData(rowIndex))
            $scope.playListModel.Name = getData(rowIndex).Name.value;
            $scope.playListModel.Id = getData(rowIndex).Id.value;
            console.log($scope.playListModel)
            $scope.addVideoHide.masterPlayList = false;
            $scope.addVideoHide.playListName = true;
            $scope.addVideoHide.playListVideos = true
        } else {
            $scope.addVideoHide.masterPlayList = false;
            $scope.addVideoHide.playListName = true;
           
        }
    }

    $scope.addPlayList = function () {
        console.log($scope.playListModel)
        //if ($scope.playListModel.Id == 0) {
            $http({
                url: '/VideoPlayer/AddPlayList',
                method: 'post',
                data: $scope.playListModel
            }).then(function (response) {

                if (response.data.id != 0) {
                    $scope.playListModel.Id = response.data.id;
                }
                
                $scope.addVideoHide.playListVideos = true
                console.log($scope.playListModel)
                $scope.dataSave($scope.playListModel);
                //$scope.addVideoHide.masterPlayList = true;
                //$scope.addVideoHide.playListName = false;
            })
        //} else {

        //}
        

        
    }

    $scope.playVideo = function () {
        var x = window.innerWidth - 22;
        var y = window.innerHeight;
        console.log(x)

        var player = videojs("videoPlayer", { controls: true, autoplay: false, width: x, height: y }, function () {
            player.src("/Videos/TestVid.mp4")
            player.on('ended', function () {
                console.log("the end")
            })
        });
    }

    


});

