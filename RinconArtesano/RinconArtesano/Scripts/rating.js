function sendRating(rating) {

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();

    var data = {
        Rating: rating,
        ProductId: productId,
        ExperienceId: experienceId
    };

    var zone = $('#RatingSection');

    $.ajax({
        url: '/Rating/SendRating',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            zone.html(data);
            check();
        },
        error: function (data) {
            alert(data.message);
        }
    });
};

check();
function check() {
    var RatingSelect = $('#RatingSelect').val();
    if (RatingSelect == 1) {
        document.getElementById("1-star").checked = true;
    }
    else if (RatingSelect == 2) {
        document.getElementById("2-star").checked = true;
    }
    else if (RatingSelect == 3) {
        document.getElementById("3-star").checked = true;
    }
    else if (RatingSelect == 4) {
        document.getElementById("4-star").checked = true;
    }
    else if (RatingSelect == 5) {
        document.getElementById("5-star").checked = true;
    }
}