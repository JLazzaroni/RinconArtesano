$('#buttonMessagePadre').click(function (e) {

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var comentario = $('#MessagePadre').val();

    var data = {
        Comentario: comentario,
        ProductId: productId,
        ExperienceId: experienceId
    };

    $.ajax({
        url: '/Messages/CreateMessagePadre',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.result == "ERROR") {
                alert("Error al enviar comentario");
            }
            else {
                location.reload();
            }
        }
    });
});

function buttonMessageHijo(messagePadreId, comentario) {
    var data = {
        Comentario: comentario.textContent,
        MessagePadreId: messagePadreId
    };

    $.ajax({
        url: '/Messages/CreateMessageHijo',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.result == "ERROR") {
                alert("Error al enviar comentario");
            }
            else {
                location.reload();
            }
        }
    });
};