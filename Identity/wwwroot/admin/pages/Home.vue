<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">{{$localizer('Dashboard')}}</h1>
    </div>

    <div class="row">
      <div v-for="link in links" class="col-sm-6 col-md-4 col-lg-3 mb-3">
        <router-link :to="link.to" class="card fast-access">
          <div class="card-body p-4 bg-dark text-center text-white">
            <i class="display-4" :class="link.icon"></i>
          </div>
          <div class="card-body">
            <h5 class="card-title">{{$localizer(link.label)}}</h5>
            <p class="card-text">{{link.description}}</p>
          </div>
        </router-link>
      </div>
    </div>

  </div>
</template>

<script>
module.exports = {
  data() {
    return {
      links: [
        {to: '/user', icon: 'icon-users', label: 'Users', description: 'Create/manage users.'},
        {to: '/client', icon: 'icon-globe', label: 'Clients', description: 'Create/manage clients.'},
        {to: '/api', icon: 'icon-box', label: 'APIs', description: 'Create/manage APIs.'},
        {to: '/role', icon: 'icon-settings', label: 'Roles', description: 'Create/manage roles.'},
      ]
    };
  }
}
</script>

<style lang="css" scoped>
.fast-access {
  color: inherit;
  text-decoration: none;
  box-shadow: 0 2px 8px transparent;
  transition: box-shadow 0.3s;
}
.fast-access:hover {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}
</style>
