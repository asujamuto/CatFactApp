async function fetchCatFact() {
    try {
        const response = await fetch('/appendCatFact');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const data = await response.json();
        console.log(data);
        document.getElementById('factDisplay').innerText = data?.message;
    } catch (error) {
        console.error('Error fetching cat fact:', error);
        document.getElementById('factDisplay').innerText = 'Error fetching cat fact.';
    }
}
async function fetchFile() {
    try {
        const response = await fetch('/fetchFile');
        
        const data = await response.json();
        console.log(data);
        document.getElementById('factDisplay').innerText = data?.message;
    } catch (error) {
        console.error('Error fetching cat fact:', error);
        document.getElementById('factDisplay').innerText = error.message;
    }
}
async function removeFile() {
    try {
        const response = await fetch('/removeCatFact');
        
        const data = await response.json();
        console.log(data);
        document.getElementById('factDisplay').innerText = data?.message;
    } catch (error) {
        console.error('Error with file removing:', error);
        document.getElementById('factDisplay').innerText = error.message;
        throw new Error(error.message);
    }
}

async function changeFileName(event) {
    event.preventDefault(); 
    

    const fileName = document.getElementById('fileName').value;
    console.log("Clicked!");

    try {
        const response = await fetch('/updateFileName', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ fileName: fileName, directoryPath: "./Files" })
        });

        const data = await response.json();

        document.getElementById('factDisplay').innerText = data.message || 'File name updated';
    } catch (error) {
        console.error('Error changing file name:', error);
        document.getElementById('factDisplay').innerText = 'Error: ' + error.message;
    }
}
