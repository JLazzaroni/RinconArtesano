
function adminBlockContenido(tipoContenido, id) {
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
}

function adminUnblockContenido(tipoContenido, id) {
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
}

function adminActivateContenido(tipoContenido, id) {
    var controller = tipoContenido == "experiencia" ? "Experiences" : (tipoContenido == "producto" ? "Products" : "Messages");
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
}

function adminDeleteContenido(tipoContenido, id) {
    var controller = tipoContenido == "experiencia" ? "Experiences" : (tipoContenido == "producto" ? "Products" : "Messages");
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
}