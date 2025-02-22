const puppeteer = require('puppeteer');
const mysql = require('mysql2');

async function scrapeItems(url, maxPages) {
    const browser = await puppeteer.launch();
    const page = await browser.newPage();
    let currentPage = 1;
    let hasNextPage = true;

    // Configuração da conexão com o banco de dados
    const connection = mysql.createConnection({
        host: '192.168.18.18',
        user: 'MobiliAR',
        password: 'MOBMOB',
        database: 'MobiliAR'
    });

    connection.connect();

    while (hasNextPage && currentPage <= maxPages) {
        console.log(`Coletando dados da página ${currentPage}`);
        await page.goto(`${url}&page=${currentPage}`, { waitUntil: 'networkidle2' });

        // Coleta de dados
        const items = await page.evaluate(() => {
            const scrapedItems = [];
            const itemElements = document.querySelectorAll('.s-main-slot .s-result-item'); // Seletor correto para os itens

            itemElements.forEach(item => {
                const nameElement = item.querySelector('h2 a span');
                const priceElement = item.querySelector('.a-price-whole');
                const linkElement = item.querySelector('h2 a');
                const imageElement = item.querySelector('img.s-image');

                const name = nameElement ? nameElement.innerText : 'Nome não disponível';
                const price = priceElement ? parseFloat(priceElement.innerText.replace('R$', '').replace(',', '.')) : 0; // Convertendo para número decimal, ou 0 se não disponível
                const link = linkElement ? linkElement.href : 'Link não disponível';
                const imageUrl = imageElement ? imageElement.src : '';

                scrapedItems.push({ name, price, link, imageUrl });
            });

            return scrapedItems;
        });

        // Inserir as informações no banco de dados
        items.forEach(item => {
            const query = 'INSERT INTO Produtos (Nome, Preço, Hyperlink, Imagem) VALUES (?, ?, ?, ?)';
            connection.query(query, [item.name, item.price || 0, item.link, item.imageUrl || ''], (err, results) => {
                if (err) throw err;
                console.log('Dados inseridos com sucesso:', results);
            });
        });

        // Verificar se há uma próxima página
        hasNextPage = await page.evaluate(() => {
            const nextButton = document.querySelector('.s-pagination-next');
            return nextButton && !nextButton.classList.contains('s-pagination-disabled');
        });

        if (hasNextPage) {
            currentPage++;
        }
    }

    await browser.close();
    connection.end();
    console.log('Scraping finalizado e dados salvos no banco de dados.');
}

// Substitua com a URL base do site e o número máximo de páginas a serem coletadas
scrapeItems('https://www.amazon.com.br/s?k=sofa', 5);
