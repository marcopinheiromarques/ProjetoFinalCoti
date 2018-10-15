function alterarRegistro(idRegistro) {

    sessionStorage.setItem('contato_funcao', 'A');

    limparCamposCadastro();

    var dado = { id: idRegistro };

    $.ajax({
        type: "GET",
        url: "/Contato/BuscarContato",
        data: dado,
        success: function (msg) {

            if (msg.sucesso === true) {

                $('#codigo'  ).val(msg.contato.IdContato);
                $('#nome'    ).val(msg.contato.Nome);
                $('#email'   ).val(msg.contato.Email);
                $('#telefone').val(msg.contato.Telefone);

                $('#codigo').prop('disabled', true);
                $('#cadastro_contato').modal('show').on('shown.bs.modal', function () {
                    $('#nome').focus();
                });
            }
            else {
                swal({
                    type: 'error',
                    title: 'Erro',
                    text: msg.mensagem
                });
            }
        },
        error: function (msg) {

            swal({
                type: 'error',
                title: 'Erro',
                text: msg
            });

        }
    });
}

function excluirRegistro(idRegistro) {

    swal({
        title: 'Confirmar exclusão?',
        text: "Esta ação não poderá ser revertida!",
        type: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.value) {

            var dado = { id: idRegistro };

            $.ajax({
                type: "POST",
                url: "/Contato/Excluir",
                data: dado,
                success: function (msg) {

                    if (msg.sucesso === true) {
                        carregaDadosContatos();

                        swal(
                          'Excluído!',
                          msg.mensagem,
                          'success'
                        );
                    }
                    else {
                        swal({
                            type: 'error',
                            title: 'Erro',
                            text: msg.mensagem
                        });
                    }                   
                },
                error: function (msg) {

                    swal({
                        type: 'error',
                        title: 'Erro',
                        text: msg
                    });
                }
            });
        }
    });

}

function carregaDadosNaTabela(dados) {

    var linha = '';

    for (var i = 0; i < dados.length; i++) {

        linha += '<tr>' +
                   '<td>' + dados[i].IdContato + '</td>' +
                   '<td>' + dados[i].Nome + '</td>' +
                   '<td>' + dados[i].Email + '</td>' +
                   '<td>' + dados[i].Telefone + '</td>' +
                   '<td class="text-center">' +
                      '<button class="btn btn-info btn-sm" onclick="alterarRegistro(' + dados[i].IdContato + ')"><i class="fas fa-pencil-alt"></i> Alterar</button>' +
                      '&nbsp;' +
                      '<button class="btn btn-danger btn-sm" onclick="excluirRegistro(' + dados[i].IdContato + ')"><i class="fas fa-trash-alt"></i> Excluir</button>' +
                   '</td>' +
                 '</tr>';
    }

    $('#lista_contatos > tbody').html(linha);

    $('#total_registros').html('<strong>Total de Registros: ' + dados.length + '</strong>');
}



function carregaDadosContatos() {    

    var usuario_logado = $('#usuario_logado').text();

    $.ajax({
        type: "GET",
        url: "/Contato/Listar",
        data: { usuarioLogado: usuario_logado },
        success: function (dados) {

            carregaDadosNaTabela(dados);
            
        },
        error: function (msg) {
            alert(msg);
        }
    });  

}

function limparCamposCadastro() {

    $('#codigo').val('');
    $('#nome').val('');
    $('#email').val('');
    $('#telefone').val('');

    $(".somente_numero").bind("keyup blur focus", function (e) {
        e.preventDefault();
        var expre = /[^\d]/g;
        $(this).val($(this).val().replace(expre, ''));
    });
}


$(document).ready(function(){

    carregaDadosContatos();

    $('#inserir_contato').click(function(){

        sessionStorage.setItem('contato_funcao', 'I');

        limparCamposCadastro();

        $('#codigo').prop('disabled', true);
        $('#cadastro_contato').modal('show').on('shown.bs.modal', function () {
            $('#nome').focus();
        });

    });

    $('#pesquisar_contato').click(function(){

        sessionStorage.setItem('contato_funcao', 'P');

        limparCamposCadastro();

        $('#codigo').prop('disabled', false);
        $('#cadastro_contato').modal('show').on('shown.bs.modal', function () {
            $('#codigo').focus();
        });
    });


    $('#todos_contatos').click(function () {

        carregaDadosContatos();        

    });


    $('#btnOK').click(function () {

        var funcao = sessionStorage.getItem('contato_funcao');

        if (funcao === 'I') {

            var dados = {
                IdContato: 0,
                Nome: $('#nome').val(),
                Email: $('#email').val(),
                Telefone: $('#telefone').val(),
                usuarioLogado: $('#usuario_logado').text()
            };

            $.ajax({
                type: "POST",
                url: "/Contato/Inserir",
                data: dados,
                success: function (msg) {                   

                    if (msg.sucesso === true) {
                        $('#cadastro_contato').modal('hide');
                        carregaDadosContatos();

                        swal(
                              'Inserido!',
                              'O registro foi inserido.',
                              'success'
                            );
                    }
                    else {

                        var lista_erros = '<ul class="alert-danger">';

                        for (var i = 0; i < msg.dados.length; i++) {

                            lista_erros += '<li>' + msg.dados[i].ErrorMessage + '</li>';

                        }

                        lista_erros += '</ul>';

                        swal({
                            type: 'warning',
                            html: lista_erros,
                            title: 'Atenção!',
                            text: 'Foram encontrados erros!'
                        });
                    }
                },
                error: function (msg) {
                    swal({
                        type: 'error',
                        title: 'Erro',
                        text: 'Não foi possível realizar a inclusão!'
                    });
                }
            });
        }
        else if (funcao === 'A') {

            var dados = {
                IdContato: $('#codigo').val(),
                Nome: $('#nome').val(),
                Email: $('#email').val(),
                Telefone: $('#telefone').val()
            };
            
            $.ajax({
                type: "POST",
                url: "/Contato/Alterar",
                data: dados,
                success: function (msg) {

                    if (msg.sucesso === true) {

                        $('#cadastro_contato').modal('hide');

                        carregaDadosContatos();

                        swal(
                          'Alterado!',
                          msg.dados,
                          'success'
                        );
                    }
                    else {

                        var lista_erros = '<ul class="text-left alert-danger">';

                        for (var i = 0; i < msg.dados.length; i++) {

                            lista_erros += '<li>' + msg.dados[i].ErrorMessage + '</li>';

                        }

                        lista_erros += '</ul>';

                        swal({
                            type: 'warning',
                            html: lista_erros,
                            title: 'Atenção!',
                            text: 'Foram encontrados erros!'
                        });

                    }

                },
                error: function (msg) {

                    swal({
                        type: 'error',
                        title: 'Erro',
                        text: msg.dados
                    });

                }
            });

        }
        else if (funcao === 'P') {

            var dados = {
                IdContato: $('#codigo').val(),
                Nome: $('#nome').val(),
                Email: $('#email').val(),
                Telefone: $('#telefone').val(),
                usuarioLogado: $('#usuario_logado').text()
            };

            $.ajax({
                type: "GET",
                url: "/Contato/Filtrar",
                data: dados,
                success: function (dados) {

                    $('#cadastro_contato').modal('hide');

                    carregaDadosNaTabela(dados);

                },
                error: function (msg) {

                    swal({
                        type: 'error',
                        title: 'Erro',
                        text: msg
                    });

                }
            });

        }

    });
});