// JavaScript source code

var createBoard = function () {
    rank = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N"];
    file = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14];

    var currentTile = {};

    var count = 0;

    for (var i = 0; i < rank.length; i++) {
        for (var j = 0; j < file.length; j++) {
            if (count % 14 == 0) {
                $("#board").append('<div class="tile smoothfade" style="clear:left"></div>');

            } else {
                $("#board").append('<div class="tile smoothfade"></div>');
            }
            $(".tile").eq(count).attr("data-gridpos", ((rank[rank.length - (i + 1)] + file[j])));

            $(".tile").eq(count).addClass("brown");
            count++;

        }
    }


}

jQuery(document).ready(function ($) {
    createBoard();
    console.log("Created board");
    tiles = $(".tile");

    $(".tile").on('click', function () {
        currentTile = $(this);
        var midY = $(this).position().top += ($(this).width() / 2);
        var midX = $(this).position().left += ($(this).width() / 2);
        var player = $("#player");



        tiles.removeClass('legal');
        console.log("X : " + midX + " || Y : " + midY);
        player.css({ "top": midY - (0.5 * player.width()), "left": midX - (0.5 * player.width()) });
        console.log("Last Clicked Tile : " + currentTile.data("gridpos"));

    })

    .on('mouseenter', function () {
        $(this).addClass('hover');
    })

    .on('mouseleave', function () {
        $(this).removeClass('hover');
    });

    var displaylegal = $("#controlbox > ul > li");


    displaylegal.on('click', function () {
        console.log($(this).attr('id'))
        legalMove($(this).attr('id'));
    })

    $('*[data-gridpos="A1"]').trigger('click');
});