// File: wwwroot/js/register.js
document
  .getElementById("registerForm")
  .addEventListener("submit", async (e) => {
    e.preventDefault();
    const name = document.getElementById("name").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    const role = document.getElementById("role").value;

    const res = await fetch("/api/auth/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name, email, password, role }),
    });
    const data = await res.json();
    if (res.ok) {
      document.getElementById("msg").innerText =
        role === "Authority"
          ? "Registered! Wait for admin verification."
          : "User registered successfully!";
      setTimeout(() => (window.location.href = "index.html"), 2000);
    } else {
      document.getElementById("msg").innerText = data;
    }
  });
