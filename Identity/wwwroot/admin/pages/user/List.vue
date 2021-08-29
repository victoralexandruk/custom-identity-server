<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">{{$localizer('Users')}}</h1>
      <div class="btn-toolbar mb-2 mb-md-0">
        <div class="input-group">
          <div class="input-group-prepend">
            <i class="input-group-text bg-white border-right-0 icon-search"></i>
          </div>
          <input type="text" class="form-control form-control-sm border-left-0" v-model="filter" :placeholder="$localizer('Search') + '...'">
        </div>
        <router-link to="/user/new" class="btn btn-sm btn-outline-secondary ml-2">
          <i class="icon-plus-circle"></i>
          {{$localizer('New')}}
        </router-link>
      </div>
    </div>
    <div v-if="!users" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <div v-if="users" class="table-responsive">
      <table class="table table-striped table-sm">
        <thead>
          <tr>
            <th>{{$localizer('UserName')}}</th>
            <th>{{$localizer('FullName')}}</th>
            <th class="text-center" style="width: 74px;">{{$localizer('Active')}}</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in filteredUsers">
            <td class="align-middle">{{user.userName}}</td>
            <td class="align-middle">{{user.fullName}}</td>
            <td class="align-middle text-center"><i class="font-weight-bold text-success" :class="{'icon-check': user.active}"></i></td>
            <td class="align-middle text-right">
              <router-link :to="'/user/' + user.id" class="btn btn-sm btn-outline-secondary"><i class="icon-edit-2"></i></router-link>
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
      users: null,
      filter: ''
    };
  },
  computed: {
    filteredUsers() {
      try {
        return this.users.filter(a => JSON.stringify(a).toLowerCase().indexOf(this.filter.toLowerCase()) !== -1);
      } catch (e) {
        return null;
      }
    }
  },
  created() {
    api.getUsers().then(users => this.users = users);
  }
}
</script>
