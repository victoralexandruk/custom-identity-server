<template>
  <div>
    <div v-if="!user" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <form v-if="user" @submit.prevent="save()">
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h3>{{$localizer('User')}}</h3>
        <div class="btn-toolbar mb-2 mb-md-0">
          <button type="submit" class="btn btn-sm btn-outline-secondary">{{$localizer('Save')}}</button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody>
            <tr>
              <th class="w-25">{{$localizer('UserName')}}</th>
              <td><input type="text" v-model="user.userName" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('FullName')}}</th>
              <td><input type="text" v-model="user.fullName" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('Email')}}</th>
              <td><input type="text" v-model="user.email" class="form-control form-control-sm" /></td>
            </tr>
            <tr>
              <th>{{$localizer('Role')}}</th>
              <td>
                <input v-if="!roles" type="text" :placeholder="$localizer('Loading...')" class="form-control form-control-sm" disabled />
                <select v-if="roles" v-model="user.role" class="form-control form-control-sm" required>
                  <option value="">{{$localizer('Select...')}}</option>
                  <option v-for="role in roles" :key="role.name" :value="role.name">{{role.displayName}}</option>
                </select>
              </td>
            </tr>
            <tr>
              <th>{{$localizer('Active')}}</th>
              <td><i class="h4" :class="{'icon-toggle-on text-success': user.active, 'icon-toggle-off text-muted': !user.active}" @click="user.active = !user.active"></i></td>
            </tr>
          </tbody>
        </table>
      </div>
    </form>
  </div>
</template>

<script>
module.exports = {
  data: function () {
    return {
      user: null,
      roles: null
    };
  },
  methods: {
    save() {
      api.saveUser(this.user).then(response => {
        this.$notyf.success(this.$localizer('Saved'));
      }).catch(error => {
        this.$notyf.error(this.$localizer('Error'));
      });
    },
    loadRoles() {
      api.getRoles().then(roles => this.roles = roles);
    }
  },
  created() {
    if (this.$route.params.id === 'new') {
      this.user = {
        userName: '',
        fullName: '',
        email: '',
        role: '',
        active: true
      };
    } else {
      api.getUser(this.$route.params.id).then(user => this.user = user);
    }
    this.loadRoles();
  }
}
</script>
