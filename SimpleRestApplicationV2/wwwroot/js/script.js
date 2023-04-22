getUsers();

document.getElementById("create-user-btn").addEventListener("click", createUser);

async function getUsers(){

    var response  = await fetch("/users", 
        {
            method: "GET",
            headers: { "Accept": "application/json" }
        }
    );

    if (response.ok) {
        var userList = await response.json();

        var table = document.getElementById("users-table");

        for (const user of userList) {
            table.appendChild(row(user));
        }
    }
    else {
        var error = await response.json();

        alert(error);
    }

   
}

async function createUser() {

    var response = await fetch("/users",
    {
        method: "POST",
        headers: { "Accept": "application/json", "Content-type": "application/json" },
        body: JSON.stringify(
            {
                name: document.getElementById("name-tb").value,
                age: document.getElementById("age-tb").value,
            }
        )
    });

    if (response.ok) {
        var newUser = await response.json();

        var table = document.getElementById("users-table");
        table.appendChild(row(newUser));   

        resetTextBoxes();
    }
    else {
        var error = await response.json();

        alert(error);
    }

   
}


async function deleteUser(userID) {

    var response = await fetch(`/users/${userID}`,
        {
            method: "DELETE",
            headers: { "Accept": "application/json" },     
        });

    if (response.ok) {
        var removedUser = await response.json();

        var rowForDelete = document.getElementById(removedUser.id);

        document.getElementById("users-table").removeChild(rowForDelete);
    }
    else {
        var error = await response.json();

        alert(error);
    }
   
    
}

async function updateUser(userID) {

    var response = await fetch(`/users/${userID}`,
        {
            method: "PUT",
            headers: { "Accept": "application/json", "Content-type": "application/json" },
            body: JSON.stringify(
                {
                    name: document.getElementById("name-tb").value,
                    age: document.getElementById("age-tb").value,
                }
            )
        });

    if (response.ok) {
        var updatedUser = await response.json();

        document.getElementById(updatedUser.id).replaceWith(row(updatedUser));

        resetTextBoxes();
    }
    else {
        var error = await response.json();

        alert(error);
    }
    

   
}

function resetTextBoxes() {
    document.getElementById("name-tb").value = "";
    document.getElementById("age-tb").value = "";
}

function row(user){

    var userRow = document.createElement("tr");
    userRow.setAttribute("id", user.id);  

    var userIDCell = document.createElement("td");
    userIDCell.innerHTML = user.id;

    var userNameCell = document.createElement("td");
    userNameCell.innerHTML = user.name;

    var userAgeCell = document.createElement("td");
    userAgeCell.innerHTML = user.age;

    var removeUserBttn = document.createElement("button");
    removeUserBttn.addEventListener("click", () => deleteUser(user.id));
    removeUserBttn.innerHTML = "Remove";

    var updateUserBttn = document.createElement("button");
    updateUserBttn.addEventListener("click", () => updateUser(user.id));
    updateUserBttn.innerHTML = "Update";

    userRow.appendChild(userIDCell);
    userRow.appendChild(userNameCell);
    userRow.appendChild(userAgeCell);
    userRow.appendChild(removeUserBttn);
    userRow.appendChild(updateUserBttn);

    return userRow;
}