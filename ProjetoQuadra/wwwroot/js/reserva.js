function gerarCalendario(dataSelecionadaIso) {

    if (!dataSelecionadaIso) dataSelecionadaIso = document.getElementById('data-input').value;

    const data = new Date(dataSelecionadaIso + 'T12:00:00'); 
    const ano = data.getFullYear();
    const mes = data.getMonth(); // 0..11
    const diaSelecionado = data.getDate();

    const nomesDosMeses = ["JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO", "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO", "NOVEMBRO", "DEZEMBRO"];
    const mesEl = document.getElementById('mes-display');
    const anoEl = document.getElementById('ano-display');
    const gridDias = document.getElementById('dias-grid');
    const baseUrlEl = document.getElementById('cal-base-url');

    if (!mesEl || !anoEl || !gridDias || !baseUrlEl) return;

    mesEl.textContent = nomesDosMeses[mes];
    anoEl.textContent = ano;

    gridDias.innerHTML = '';

    const primeiroDia = new Date(ano, mes, 1, 12).getDay();
    const ultimoDia = new Date(ano, mes + 1, 0, 12).getDate();

    for (let i = 0; i < primeiroDia; i++) {
        const li = document.createElement('li');
        li.classList.add('fora');
        gridDias.appendChild(li);
    }

    for (let dia = 1; dia <= ultimoDia; dia++) {
        const li = document.createElement('li');
        li.textContent = dia;

        if (dia === diaSelecionado) {
            li.classList.add('selecionado');
        }

        const hoje = new Date();
        if (hoje.getFullYear() === ano && hoje.getMonth() === mes && hoje.getDate() === dia) {
            li.classList.add('hoje');
        }

        li.addEventListener('click', function () {
            const mm = String(mes + 1).padStart(2, '0');
            const dd = String(dia).padStart(2, '0');
            const novaData = `${ano}-${mm}-${dd}`;

            const template = baseUrlEl.getAttribute('data-url-template');
            const url = template.replace('__DATE__', novaData);
            window.location.href = url;
        })

        gridDias.appendChild(li);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('data-input');
    const initial = input ? input.value : null;
    gerarCalendario(initial);
})