// File: wwwroot/js/admin-dashboard.js
const token = localStorage.getItem("token");
if (!token) window.location.href = "index.html";

const container = document.getElementById("pendingContainer");

async function fetchPendingAuthorities() {
  const res = await fetch("/api/admin/pending-authorities", {
    headers: { Authorization: `Bearer ${token}` },
  });
  if (res.status === 401) {
    alert("Unauthorized! Admin only.");
    return;
  }
  const data = await res.json();
  container.innerHTML = "";

  if (data.length === 0) {
    container.innerHTML = "<p>No pending authorities.</p>";
    return;
  }

  data.forEach((auth) => {
    const div = document.createElement("div");
    div.innerHTML = `
            <p><strong>${auth.name}</strong> (${auth.email})</p>
            <button class="approveBtn" data-id="${auth.id}">Approve</button>
        `;
    container.appendChild(div);
  });

  document.querySelectorAll(".approveBtn").forEach((btn) => {
    btn.addEventListener("click", async () => {
      const id = btn.dataset.id;
      const res = await fetch(`/api/admin/verify/${id}`, {
        method: "PUT",
        headers: { Authorization: `Bearer ${token}` },
      });
      if (res.ok) {
        alert("Authority verified!");
        fetchPendingAuthorities();
      } else {
        alert("Failed to verify authority.");
      }
    });
  });
}

// Initial fetch
fetchPendingAuthorities();
