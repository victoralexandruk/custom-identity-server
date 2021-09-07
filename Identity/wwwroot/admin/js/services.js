const websiteUrl = location.origin + location.pathname.replace(/\/$/, '');

const apiUrls = {};

apiUrls.account = websiteUrl + '/../account';
apiUrls.me = websiteUrl + '/../api/v1/me';
apiUrls.admin = websiteUrl + '/../api/v1/admin';

const auth = {
  redirectToLogin(returnUrl) {
    const redirectUri = `/admin/#${returnUrl}`;
    location.href = `${apiUrls.account}/Login?ReturnUrl=${encodeURIComponent(redirectUri)}`;
  },
  redirectToLogout() {
    location.href = `${apiUrls.account}/logout`;
  }
};

/* api client ================================================================*/
const api = {
  ajax(options) {
    return new Promise((resolve, reject) => {
      $.ajax(options).done(resolve).fail(reject);
    });
  },
  async getMyUser() {
    try {
      return await this.ajax({
        type: "GET",
        url: `${apiUrls.me}`,
        dataType: "json"
      });
    } catch {
      return null;
    }
  },
  /* Users ================================================================== */
  getUsers() {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/user`,
      dataType: "json"
    });
  },
  getUser(id) {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/user/${id}`,
      dataType: "json"
    });
  },
  saveUser(user) {
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/user`,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(user)
    });
  },
  saveUsers(users) {
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/user/batch`,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(users)
    });
  },
  /* Clients ================================================================ */
  getClients(withPermissions) {
    withPermissions = withPermissions || false;
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/client?withPermissions=${withPermissions}`,
      dataType: "json"
    });
  },
  getClient(id) {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/client/${id}`,
      dataType: "json"
    });
  },
  saveClient(client) {
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/client`,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(client)
    });
  },
  deleteClient(id) {
    return this.ajax({
      type: "DELETE",
      url: `${apiUrls.admin}/client/${id}`,
    });
  },
  uploadClientLogo(id, file) {
    var formData = new FormData();
    formData.append('formFile', file);
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/client/${id}/logo`,
      contentType: false,
      processData: false,
      data: formData
    });
  },
  /* Resources ============================================================== */
  getApiResources() {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/resource/api`,
      dataType: "json"
    });
  },
  getApiResource(id) {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/resource/api/${id}`,
      dataType: "json"
    });
  },
  saveApiResource(apiResource) {
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/resource/api`,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(apiResource)
    });
  },
  deleteApiResource(id) {
    return this.ajax({
      type: "DELETE",
      url: `${apiUrls.admin}/resource/api/${id}`,
    });
  },
  /* Roles ================================================================== */
  getRoles() {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/role`,
      dataType: "json"
    });
  },
  getRole(id) {
    return this.ajax({
      type: "GET",
      url: `${apiUrls.admin}/role/${id}`,
      dataType: "json"
    });
  },
  saveRole(role) {
    return this.ajax({
      type: "PUT",
      url: `${apiUrls.admin}/role`,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(role)
    });
  },
  deleteRole(id) {
    return this.ajax({
      type: "DELETE",
      url: `${apiUrls.admin}/role/${id}`,
    });
  },
};
