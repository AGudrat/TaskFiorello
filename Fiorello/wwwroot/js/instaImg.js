const instagram = document.getElementById('instagram');
const instaImg = instagram.firstChild.nextSibling;
$(document).ready(async function () {
    const token = "IGQVJWclNVWjktNmU4S0tqRlIteU5aWVJ5S2ZAYM3ZA0MGhGbC16OTNvVHdTUlcxQ2JiN0dXN1E3OHpFWW5lUUF6OElCbUhVWWNHb012TkNHSXdKNXp5clNCeVc2MlI5eDR1M0lLRmhmSlg1OEczRTlZAQwZDZD";
    const url = "https://graph.instagram.com/me/media?access_token=" + token + "&fields=media_url,media_type,caption,permalink";
    $.get(url).then(function (responce) {
        data = responce.data;
        data.forEach(element => {
            instaImg.innerHTML += `<div><img src="${element.media_url}" class="img-fluid" alt=""></div>`;
        });
    })
})