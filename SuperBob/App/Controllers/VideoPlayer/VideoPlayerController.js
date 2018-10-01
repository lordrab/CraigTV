
app.controller("videoPlayerCtrl", function ($scope, $http, $uibModal, $compile, inputModelService, yesNoPopupService) {

    $scope.playListModel = { Name: '', Id: 0 };
    $scope.dataSave = '';
    $scope.addVideoHide = { masterPlayList: true, playListName: false, playListVideos: false };

    $scope.showVideoList = true;
    $scope.showPlayer = false;
    $scope.showPlayList = false;
    $scope.videoData = [];
    $scope.playListId = 0;
    $scope.deleteId = 0;
    $scope.currentVideoPlaying = 0;
    $scope.bob = 14;
    $scope.videoPLayList = [{ Id: 0, PlayListName: '' }];
    $scope.hideList = true;

    var x = window.innerWidth - 22;
    var y = window.innerHeight - 155;

    $scope.buttonModel = {
        buttonData:
            [
                {
                    title: 'Add', class: 'btn sharedbutton-edit', show: true, buttonClickFunction: function (rowIndex, buttonIndex) {
                        console.log(rowIndex)
                        console.log(buttonIndex)
                        return {
                            action: 'toggle',
                            button1: 0,
                            button2: 1
                        };
                    }              
                },
                {
                    title: 'Delete', class: 'btn sharedbutton-delete', show: false, buttonClickFunction: function (rowIndex, buttonIndex) {
                        console.log(rowIndex)
                        console.log(buttonIndex)
                        return {
                            action: 'toggle',
                            button1: 1,
                            button2: 0
                        };
                    }
                }]         
    };

    $scope.filterModel = {  
        filterStartId: 0,
        filterList: [],
        filterPropertyName: 'CatagoryId',
        listUrl: '/VideoLibrary/GetGenreList',
        getModelPropertyName: 'CatagoryList',
        CustomFilterList: [{
            Id: 0, Name: 'Show PlayList', Custom: true, hideShowList: false,executeFunction: function (data) {
                //console.log(data)
                $scope.showPlayList = true;
                $scope.hideList = false;
            }
        }]
    };

    $http({
        url: "/VideoPlayer/GetPLayLists",
        method: 'get'
    }).then(function (response) {
        //console.log(response)
        if (response.data.length != 0) {
            $scope.videoPLayList = response.data;
            $scope.playListId = $scope.videoPLayList[0].Id;
            $scope.getPlayList($scope.playListId);
        }
    });

    $scope.getPlayList = function (playListId) {
        if ($scope.playListId != 0) {
            $http({
                url: "/VideoPlayer/GetCurrentVideoList",
                method: 'post',
                data: { playListId: playListId }
            }).then(function (response) {
                //console.log(response)
                $scope.videoData = response.data.VideoList;
            });
        }
    };

    $scope.playListChanged = function () {
        $scope.getPlayList($scope.playListId);
    };

    $scope.playVideo = function (index) {
        
        $scope.showVideoList = false;
        $scope.showPlayer = true;
        $scope.player;
        $scope.currentVideoPlaying = index;
        $scope.player = videojs("videoPlayer", { controls: true, autoplay: true, height: y }, function () {
            $scope.player.src("/Videos/" + $scope.videoData[index].FileName);
            $scope.player.on('ended', function () {
                var cId = $scope.currentVideoPlaying;
                cId++;
                
                if (cId < $scope.videoData.length) {
                    $scope.player.src("/Videos/" + $scope.videoData[cId].FileName);
                    $scope.currentVideoPlaying = cId;
                    console.log($scope.videoData[cId].FileName)
                    console.log($scope.videoData)
                } else {
                    $scope.showVideoList = true;
                    $scope.showPlayer = false;
                    console.log($scope.showVideoList);
                }
            });
        });
    };

    $scope.showList = function () {
        $scope.showVideoList = true;
        $scope.showPlayer = false;
        $scope.player.pause();
    };

    $scope.addPlayList = function () {
        inputModelService.InputData($scope.savePlayList, "Enter Playlist Name");
    };

    $scope.savePlayList = function (playlistName) {
        
        $http({
            url: '/VideoPlayer/AddPlayList',
            method: 'post',
            data: {
                playList: playlistName
            }
        }).then(function (response) {
            console.log(response)
            $scope.videoPLayList.push({ Id: response.data.id, PlayListName: playlistName });
            $scope.playListId = response.data.id;
        })
    }

    $scope.addVideo = function () {
        $scope.hideList = true;
        $scope.showPlayList = false;
        //$http({
        //    url: "/VideoPlayer/GetVideoLibrary",
        //    method: 'get'
        //}).then(function (response) {
        //    console.log(response)
        //    var videoList = [];
        //    for (i = 0; i < response.data.length; i++) {
        //        videoList.push({ Id: response.data[i].Id, Text: response.data[i].Title });
        //    }
        //    inputModelService.dropDownData($scope.addVideoSave, "Enter Playlist Name", videoList);
        //});
    };

    $scope.addVideoSave = function (index) {
        var model = {
            PlayListId: $scope.playListId,
            LibraryId: index
        };
        console.log($scope.playListId)
        $http({
            url: '/VideoPlayer/AddPlayListVideo',
            method: 'post',
            data: model
        }).then(function (response) {
            $scope.videoData.push(response.data.AddedVideo)

        });
    };   

    $scope.deleteVideo = function (index) {
        $scope.deleteId = index;
        yesNoPopupService.YesNoPopup($scope.comfirmDeleteVideo);

    };

    $scope.comfirmDeleteVideo = function () {
        console.log($scope.deleteId)
        var model = {
            Id: $scope.videoData[$scope.deleteId].Id
        };

        $http({
            url: '/VideoPlayer/DeleteFromPlayList',
            method: 'post',
            data: model
        }).then(function (response) {
            //console.log(response)
            if (response.data.Success) {
                $scope.videoData.splice($scope.deleteId, 1);
            }
        });
    };
});

