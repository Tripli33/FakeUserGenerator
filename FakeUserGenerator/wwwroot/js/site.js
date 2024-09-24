function changeRegion() {
    let selectedRegion = document.getElementById("regionSelect").value;
    window.location.href = `?Region=${selectedRegion}`;
}

function setSelectedRegion() {
    const urlParams = new URLSearchParams(window.location.search);
    const region = urlParams.get('Region');

    if (region) {
        const regionSelect = document.getElementById("regionSelect");
        regionSelect.value = region; 
    }
}

document.getElementById("regionSelect").addEventListener("change", changeRegion);
window.onload = setSelectedRegion; 