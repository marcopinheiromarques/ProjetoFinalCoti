function RetornaDataFormatada(valor) {

    var retorno = valor.replace(/[^0-9 +]/g, '');
    var data = new Date(parseInt(retorno));

    return data.getDate() + "/" + (data.getMonth() + 1) + "/" + data.getFullYear();
}


function alterarRegistro(idRegistro) {

    sessionStorage.setItem('tarefa_funcao', 'A');

    limparCamposCadastro();

    var dado = { id: idRegistro };

    $.ajax({
        type: "GET",
        url: "/Tarefa/BuscarTarefa",
        data: dado,
        success: function (msg) {

            if (msg.sucesso === true) {

                $('#codigo').val(msg.tarefa.IdTarefa);
                $('#nome').val(msg.tarefa.Nome);
                $('#data_entrega').val(RetornaDataFormatada(msg.tarefa.DataEntrega));
                $('#descricao').val(msg.tarefa.Descricao);

                $('#codigo').prop('disabled', true);
                $('#cadastro_tarefa').modal('show').on('shown.bs.modal', function () {
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
                url: "/Tarefa/Excluir",
                data: dado,
                success: function (msg) {

                    if (msg.sucesso === true) {
                        carregaDadosTarefas();

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
            '<td>' + dados[i].IdTarefa + '</td>' +
            '<td>' + dados[i].Nome + '</td>' +
            '<td>' + RetornaDataFormatada(dados[i].DataEntrega) + '</td>' +
            '<td>' + dados[i].Descricao + '</td>' +
            '<td class="text-center">' +
            '<button class="btn btn-info btn-sm" onclick="alterarRegistro(' + dados[i].IdTarefa + ')"><i class="fas fa-pencil-alt"></i> Alterar</button>' +
            '&nbsp;' +
            '<button class="btn btn-danger btn-sm" onclick="excluirRegistro(' + dados[i].IdTarefa + ')"><i class="fas fa-trash-alt"></i> Excluir</button>' +
            '</td>' +
            '</tr>';
    }

    $('#lista_tarefas > tbody').html(linha);

    $('#total_registros').html('<strong>Total de Registros: ' + dados.length + '</strong>');
}

function carregaDadosTarefas() {

    var usuario_logado = $('#usuario_logado').text();

    $.ajax({
        type: "GET",
        url: "/Tarefa/Listar",
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
    $('#data_entrega').val('');
    $('#descricao').val('');
    
}

$(document).ready(function () {

    carregaDadosTarefas();

    $('.data').mask("00/00/0000", { placeholder: "__/__/____" });   

    $('#inserir_tarefa').click(function () {

        sessionStorage.setItem('tarefa_funcao', 'I');

        limparCamposCadastro();

        $('#codigo').prop('disabled', true);
        $('#cadastro_tarefa').modal('show').on('shown.bs.modal', function () {
            $('#nome').focus();
        });

    });

    $('#pesquisar_tarefa').click(function () {

        sessionStorage.setItem('tarefa_funcao', 'P');

        limparCamposCadastro();

        $('#codigo').prop('disabled', false);
        $('#cadastro_tarefa').modal('show').on('shown.bs.modal', function () {
            $('#codigo').focus();
        });
    });

    $('#todos_tarefas').click(function () {

        carregaDadosTarefas();

    });

    $('#btnOK').click(function () {

        var dados;
        var funcao = sessionStorage.getItem('tarefa_funcao');

        if (funcao === 'I') {

            dados = {
                IdTarefa: 0,
                Nome: $('#nome').val(),
                DataEntrega: $('#data_entrega').val(),
                Descricao: $('#descricao').val(),
                usuarioLogado: $('#usuario_logado').text()
            };

            $.ajax({
                type: "POST",
                url: "/Tarefa/Inserir",
                data: dados,
                success: function (msg) {

                    if (msg.sucesso === true) {
                        $('#cadastro_tarefa').modal('hide');
                        carregaDadosTarefas();

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

            dados = {
                IdTarefa: $('#codigo').val(),
                Nome: $('#nome').val(),
                DataEntrega: $('#data_entrega').val(),
                Descricao: $('#descricao').val()                
            };

            $.ajax({
                type: "POST",
                url: "/Tarefa/Alterar",
                data: dados,
                success: function (msg) {

                    if (msg.sucesso === true) {

                        $('#cadastro_tarefa').modal('hide');

                        carregaDadosTarefas();

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

            dados = {
                IdTarefa: $('#codigo').val(),
                Nome: $('#nome').val(),
                DataEntrega: $('#data_entrega').val(),
                Descricao: $('#descricao').val(),
                usuarioLogado: $('#usuario_logado').text()
            };

            $.ajax({
                type: "POST",
                url: "/Tarefa/Filtrar",
                data: dados,
                success: function (dados) {

                    $('#cadastro_tarefa').modal('hide');

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