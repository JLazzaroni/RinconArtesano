function messageAdd(messagePadreId, comentario) {
    var comentario;
    var padreId;
    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();

    if (messagePadreId == 0) {
        comentario = $('#MessagePadre').val();
    }
    else {
        comentario = comentario.value;
        padreId = messagePadreId;
    }

    var data = {
        Message: comentario,
        ProductId: productId,
        ExperienceId: experienceId,
        IdComentarioPadre: padreId
    };

    var zone = $('#MessageSection');

    $.ajax({
        url: '/Messages/CreateMessage',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.result == "ERROR") {
                alert("Error al enviar comentario");
            }
            else {
                if (data.status == "Error")
                {
                    alert(data.message);
                }
                else
                {
                    zone.html(data);
                }
            }
        }
    });
};