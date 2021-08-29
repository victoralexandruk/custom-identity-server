<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">{{$localizer('Dashboard')}}</h1>
    </div>

    <div class="row">
      <div v-for="link in links" class="col-sm-6 col-md-4 col-lg-3 mb-3">
        <div class="card">
          <div class="card-body p-4 bg-dark text-center text-white">
            <i class="display-4" :class="link.icon"></i>
          </div>
          <div class="card-body">
            <h5 class="card-title">{{$localizer(link.label)}}</h5>
            <p class="card-text">{{link.description}}</p>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
module.exports = {
  data() {
    return {
      links: [
        {icon: 'icon-users', label: 'Users', description: 'Create/manage users.'},
        {icon: 'icon-globe', label: 'Clients', description: 'Create/manage clients.'},
      ]
    };
  }
}
</script>
