
app.controller("videoPlayerCtrl", function ($scope, $http, $uibModal,$compile) {

    $scope.playListModel = { Name: '', Id: 0 };
    $scope.dataSave = '';
    $scope.addVideoHide = { masterPlayList: true, playListName: false, playListVideos: false }

    $scope.bob = { id: 0 }

  
    $scope.addVideoList = function (index, rowIndex, saveData,getData) {
        $scope.dataSave = saveData;

        var html = `<div class="form-group">
                        <label>PlayList Videos</label>
                        <super-list mvccontroller="VideoPlayer" display-List-Data="bob" list-Data-Method="GetVideoPLayList" edit-Data-Method="EditPlayListData"></super-list>
                    </div>`
      
        if (index != 0) {
            console.log(getData(rowIndex))
            $scope.playListModel.Name = getData(rowIndex).Name.value;
            $scope.playListModel.Id = getData(rowIndex).Id.value;
            $scope.bob.id = getData(rowIndex).Id.value;
            console.log($scope.playListModel)
            $scope.addVideoHide.masterPlayList = false;
            $scope.addVideoHide.playListName = true;
            $scope.addVideoHide.playListVideos = true
        } else {
            $scope.addVideoHide.masterPlayList = false;
            $scope.addVideoHide.playListName = true;
           
        }
        $("#bob").html($compile(html)($scope))
    }

    $scope.addPlayList = function () {
        
            $http({
                url: '/VideoPlayer/AddPlayList',
                method: 'post',
                data: $scope.playListModel
            }).then(function (response) {

                if (response.data.id != 0) {
                    $scope.playListModel.Id = response.data.id;
                }                
                $scope.addVideoHide.playListVideos = true               
                $scope.dataSave($scope.playListModel);
                //$scope.addVideoHide.masterPlayList = true;
                //$scope.addVideoHide.playListName = false;
            })                    
    }

    $scope.closeVideoPLayList = function () {
        $scope.addVideoHide.masterPlayList = true;
        $scope.addVideoHide.playListName = false;
        $scope.addVideoHide.playListVideos = false
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

