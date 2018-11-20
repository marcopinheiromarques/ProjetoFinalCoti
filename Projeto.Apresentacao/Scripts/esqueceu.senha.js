function alterarSenha() {

    var email = $('#inputEmail').val();

    if (email.trim() == "") {

        $('#mensagem_validacao').html('<p>O email é obrigatório!</p>');
        $('#inputEmail').focus();

    }
    else {

        $('#mensagem_validacao').html('');
        $('#mensagem_confirmacao').html('');

        //exibe o carregamento
        $("#loading").addClass("fa");
        $("#loading").addClass("fa-spinner");
        $("#loading").addClass("fa-spin");

        $.ajax({
            type: "POST",
            url: "/Usuario/AlterarSenhaAutomaticamente",
            data: { email: email },
            success: function (msg) {

                $("#loading").removeClass("fa");
                $("#loading").removeClass("fa-spinner");
                $("#loading").removeClass("fa-spin");

                if (msg.sucesso == true) {

                    $('#mensagem_confirmacao').html('<p>' + msg.mensagem + '</p>');

                }
                else {

                    $('#mensagem_validacao').html('<p>' + msg.mensagem + '</p>');

                }
            },
            error: function (msg) {

                $("#loading").removeClass("fa");
                $("#loading").removeClass("fa-spinner");
                $("#loading").removeClass("fa-spin");

                $('#mensagem_validacao').html('<p>' + msg.mensagem + '</p>');
            }
        });
    }
}