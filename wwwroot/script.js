// script.js
const API_BASE = '';  // si front+API même origine
let token = null;

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('loginBtn').addEventListener('click', login);
    document.getElementById('loadBtn').addEventListener('click', loadOpenings);
});

// 1) Authentification
async function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    try {
        const res = await fetch(`${API_BASE}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });
        if (!res.ok) throw new Error(await res.text());

        const { token: jwt } = await res.json();
        token = jwt;

        document.getElementById('auth').style.display = 'none';
        document.getElementById('content').style.display = 'block';
    } catch (err) {
        alert(`Erreur de connexion : ${err.message}`);
    }
}

// 2) Chargement des ouvertures et rendu des cartes
async function loadOpenings() {
    try {
        const res = await fetch(`${API_BASE}/api/openings`, {
            headers: { 'Authorization': 'Bearer ' + token }
        });
        if (!res.ok) throw new Error(await res.text());

        const list = await res.json();
        const container = document.getElementById('cardsContainer');
        container.innerHTML = '';

        list.forEach(o => {
            const card = document.createElement('div');
            card.className = 'card';

            // Titre de l'ouverture
            const title = document.createElement('h3');
            title.textContent = `${o.eco.toUpperCase()} – ${o.name}`;

            // Fréquence
            const freq = document.createElement('p');
            freq.textContent = `Fréquence : ${o.frequency}`;

            // échiquier SVG
            const boardSize = 160;
            const svgNS = 'http://www.w3.org/2000/svg';
            const svg = document.createElementNS(svgNS, 'svg');
            svg.setAttribute('width', boardSize);
            svg.setAttribute('height', boardSize);
            svg.classList.add('board');

            // **Label Winrate** 
            const winrateLabel = document.createElement('p');
            winrateLabel.textContent = 'Winrate';
            winrateLabel.classList.add('winrate-label');
            // Vous pouvez styler .winrate-label en CSS (taille, alignement...)

            // doughnut win‑rate
            const canvas = document.createElement('canvas');
            canvas.width = 150;
            canvas.height = 150;

            // Assemblage de la carte
            card.append(title, freq, svg, winrateLabel, canvas);
            container.appendChild(card);

            const moves = o.moves.split(' ');

            drawBoard(svg, boardSize);
            drawArrows(svg, moves, boardSize);
            drawWinRateChart(canvas, o.winRate);
        });
    } catch (err) {
        alert(`Impossible de charger les ouvertures : ${err.message}`);
    }
}

// 3) Fonctions utilitaires SVG
function drawBoard(svg, size) {
    const cell = size / 8;
    svg.innerHTML = '';
    for (let r = 0; r < 8; r++) {
        for (let f = 0; f < 8; f++) {
            const rect = document.createElementNS(svg.namespaceURI, 'rect');
            rect.setAttribute('x', f * cell);
            rect.setAttribute('y', r * cell);
            rect.setAttribute('width', cell);
            rect.setAttribute('height', cell);
            rect.setAttribute('fill', (r + f) % 2 === 0 ? '#f0d9b5' : '#b58863');
            svg.appendChild(rect);
        }
    }
    const defs = document.createElementNS(svg.namespaceURI, 'defs');
    const marker = document.createElementNS(svg.namespaceURI, 'marker');
    marker.setAttribute('id', 'arrowhead');
    marker.setAttribute('markerWidth', 6);
    marker.setAttribute('markerHeight', 6);
    marker.setAttribute('refX', 0);
    marker.setAttribute('refY', 3);
    marker.setAttribute('orient', 'auto');
    const poly = document.createElementNS(svg.namespaceURI, 'polygon');
    poly.setAttribute('points', '0 0, 6 3, 0 6');
    poly.setAttribute('fill', 'red');
    marker.appendChild(poly);
    defs.appendChild(marker);
    svg.appendChild(defs);
}

function drawArrows(svg, moves, size) {
    const cell = size / 8;
    moves.forEach((mv, i) => {
        const to = mv.slice(-2);
        let from;
        if (i === 0) {
            from = to[0] + '2';
        } else if (i === 1) {
            from = to[0] + '7';
        } else {
            from = moves[i-1].slice(-2);
        }
        const a = algebraicToCoord(from, cell);
        const b = algebraicToCoord(to, cell);

        const line = document.createElementNS(svg.namespaceURI, 'line');
        line.setAttribute('x1', a.x);
        line.setAttribute('y1', a.y);
        line.setAttribute('x2', b.x);
        line.setAttribute('y2', b.y);
        line.setAttribute('stroke', 'red');
        line.setAttribute('stroke-width', 2);
        line.setAttribute('marker-end', 'url(#arrowhead)');
        svg.appendChild(line);
    });
}

function algebraicToCoord(square, cell) {
    const file = square.charCodeAt(0) - 97;
    const rank = 8 - parseInt(square[1], 10);
    return {
        x: file * cell + cell/2,
        y: rank * cell + cell/2
    };
}

// 4) Doughnut win‑rate avec Chart.js
function drawWinRateChart(canvas, winRate) {
    new Chart(canvas.getContext('2d'), {
        type: 'doughnut',
        data: {
            labels: ['Win', 'Lose'],
            datasets: [{
                data: [winRate, 100 - winRate]
            }]
        },
        options: {
            cutout: '70%',
            plugins: {
                // titre interne (optionnel si vous préférez le DOM)
                title: {
                    display: false
                },
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: ctx => `${ctx.label} : ${ctx.parsed}%`
                    }
                }
            },
            responsive: true
        }
    });
}
