﻿$('#denunciasBtn').click(function (e) {
    e.preventDefault();

    var productId = $('#ProductId').val();
    var experienceId = $('#ExperienceId').val();
    var _comentario = $('input:radio[name=denunciaRespuesta]:checked').val();
    var comentario = null;

    switch (_comentario) {
        case "noartesania":
            comentario = "No es una artesania";
            break;
        case "sexual":
            comentario = "Contenido sexual";
            break;
        case "spam":
            comentario = "Es spam";
            break;
        case "comentario":
            comentario = $('#denunciaRespuestaComentario').val();
    }

    var data = {
        Comentario: comentario,
        ProductId: productId,
        ExperienceId: experienceId
    };
    $.ajax({
        url: '/Denuncias/Create',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            alert(data.message);
            init();
            location.reload();
        },
        error: function (data) {
            alert(data.message);
        }
    });
    function init()
    {
        //Reiniciar los valores por default de los campos
        document.getElementById("denunciaRespuestaComentario").value = "";
        $("#radioComentario").prop("checked", true);
        //Ocultar el modal
        $('#modalDenuncias').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    }
});

$("input:radio").change(function () {
    var value = $("input:radio:checked").val();

    if (value == "comentario")
    {
        document.getElementById("denunciaRespuestaComentario").disabled = false;            
    }
    else
    {
        document.getElementById("denunciaRespuestaComentario").disabled = true;
        document.getElementById("denunciaRespuestaComentario").value = "";
    }
});
