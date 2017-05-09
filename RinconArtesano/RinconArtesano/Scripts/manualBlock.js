$('#manualBlockBtn').click(function (e) {
    e.preventDefault();

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var tipoContenido = typeof productId !== "undefined"? "producto" : "experiencia";
    var id = typeof productId !== "undefined" ? productId : experienceId;
    
    var data = {
        tipoContenido: tipoContenido,
        id: id
    };
    $.ajax({
        url: '/Denuncias/manualBlock',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.message);
            location.reload();
        },
        error: function (data) {
            alert('Error');
        }
    });
});
$('#manualUnblockBtn').click(function (e) {
    e.preventDefault();

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var tipoContenido = typeof productId !== "undefined" ? "producto" : "experiencia";
    var id = typeof productId !== "undefined" ? productId : experienceId;

    var data = {
        tipoContenido: tipoContenido,
        id: id
    };
    $.ajax({
        url: '/Denuncias/manualUnblock',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.message);
            location.reload();
        },
        error: function (data) {
            alert('Error');
        }
    });
});
$('#manualDeleteBtn').click(function (e) {
    e.preventDefault();

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var controller = typeof productId !== "undefined" ? "Products" : "Experiences";
    var id = typeof productId !== "undefined" ? productId : experienceId;

    var data = { id: id };
    $.ajax({
        url: '/' + controller + '/manualDelete',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.message);
            location.reload();
        },
        error: function (data) {
            alert('Error');
        }
    });
});
$('#manualActivateBtn').click(function (e) {
    e.preventDefault();

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var controller = typeof productId !== "undefined" ? "Products" : "Experiences";
    var id = typeof productId !== "undefined" ? productId : experienceId;

    var data = { id: id };
    $.ajax({
        url: '/' + controller + '/manualActivate',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.message);
            location.reload();
        },
        error: function (data) {
            alert('Error');
        }
    });
});