<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">{{$localizer('Roles')}}</h1>
      <div class="btn-toolbar mb-2 mb-md-0">
        <div class="input-group">
          <div class="input-group-prepend">
            <i class="input-group-text bg-white border-right-0 icon-search"></i>
          </div>
          <input type="text" class="form-control form-control-sm border-left-0" v-model="filter" :placeholder="$localizer('Search') + '...'">
        </div>
        <router-link to="/role/new" class="btn btn-sm btn-outline-secondary ml-2">
          <i class="icon-plus-circle"></i>
          {{$localizer('New')}}
        </router-link>
      </div>
    </div>
    <div v-if="!roles" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <div v-if="roles" class="table-responsive">
      <table class="table table-striped table-sm">
        <thead>
          <tr>
            <th>{{$localizer('DisplayName')}}</th>
            <th>{{$localizer('Name')}}</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="role in filteredRoles">
            <td class="align-middle">{{role.displayName}}</td>
            <td class="align-middle">{{role.name}}</td>
            <td class="align-middle text-right">
              <router-link :to="'/role/' + role.id" class="btn btn-sm btn-outline-secondary"><i class="icon-edit-2"></i></router-link>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
module.exports = {
  data() {
    return {
      roles: null,
      filter: ''
    };
  },
  computed: {
    filteredRoles() {
      try {
        return this.roles.filter(a => JSON.stringify(a).toLowerCase().indexOf(this.filter.toLowerCase()) !== -1);
      } catch (e) {
        return null;
      }
    }
  },
  created() {
    api.getRoles().then(roles => this.roles = roles);
  }
}
</script>
