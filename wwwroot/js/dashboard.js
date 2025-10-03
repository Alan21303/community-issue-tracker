// File: wwwroot/js/dashboard.js
const token = localStorage.getItem("token");
if (!token) window.location.href = "index.html";

const container = document.getElementById("reportsContainer");

async function fetchReports() {
  const status = document.getElementById("statusFilter").value;
  const category = document.getElementById("categoryFilter").value;
  const tag = document.getElementById("tagFilter").value;

  let url = `/api/reports?status=${status}&category=${category}&tag=${tag}`;
  const res = await fetch(url, {
    headers: { Authorization: `Bearer ${token}` },
  });
  const data = await res.json();
  container.innerHTML = "";
  data.forEach((r) => {
    const div = document.createElement("div");
    div.innerHTML = `
            <h3>${r.title} [${r.status}]</h3>
            <p>${r.description}</p>
            <p>Category: ${r.category} | Tags: ${r.reportTags
      .map((t) => t.tag.name)
      .join(", ")}</p>
            <p>Location: ${r.locationText}</p>
            <p>Submitted by: ${r.createdByUser.name}</p>
            ${r.media
              .map((m) =>
                m.mediaType == "image"
                  ? `<img src="${m.filePath}" width="200">`
                  : `<video width="320" controls><source src="${m.filePath}"></video>`
              )
              .join("")}
            <br><br>
        `;
    // If Authority, show status update
    if (r.status !== "Resolved" && isAuthority()) {
      const select = document.createElement("select");
      ["Submitted", "Reviewed", "Resolved"].forEach((s) => {
        const opt = document.createElement("option");
        opt.value = s;
        opt.innerText = s;
        if (s == r.status) opt.selected = true;
        select.appendChild(opt);
      });
      const btn = document.createElement("button");
      btn.innerText = "Update Status";
      btn.onclick = async () => {
        await fetch(`/api/reports/${r.id}/status`, {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({ status: select.value }),
        });
        fetchReports();
      };
      div.appendChild(select);
      div.appendChild(btn);
    }

    container.appendChild(div);
  });
}

function isAuthority() {
  const payload = JSON.parse(atob(token.split(".")[1]));
  return (
    payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ===
    "Authority"
  );
}

document.getElementById("applyFilters").addEventListener("click", fetchReports);
document
  .getElementById("newReportBtn")
  .addEventListener("click", () => (window.location.href = "report-form.html"));

fetchReports();
