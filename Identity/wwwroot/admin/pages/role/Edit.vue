<template>
  <div>
    <div v-if="!role" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <form v-if="role" @submit.prevent="save()">
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h3>{{$localizer('Role')}}</h3>
        <div class="btn-toolbar mb-2 mb-md-0">
          <button type="submit" class="btn btn-sm btn-outline-secondary">{{$localizer('Save')}}</button>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody>
            <tr>
              <th class="w-25">{{$localizer('Name')}}</th>
              <td><input type="text" v-model="role.name" class="form-control form-control-sm" required /></td>
            </tr>
            <tr>
              <th>{{$localizer('DisplayName')}}</th>
              <td><input type="text" v-model="role.displayName" class="form-control form-control-sm" required /></td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Permissions -->
      <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center">
        <h5 class="mt-3">{{$localizer('Permissions')}}</h5>
        <div class="btn-toolbar mb-2 mb-md-0">
          <div class="input-group">
            <div class="input-group-prepend">
              <i class="input-group-text bg-white border-right-0 icon-search"></i>
            </div>
            <input type="text" class="form-control form-control-sm border-left-0" v-model="permissionsFilter" :placeholder="$localizer('Search') + '...'">
          </div>
        </div>
      </div>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody v-for="client in filteredClients">
            <tr>
              <th colspan="3" class="py-2">{{client.clientName}}</th>
            </tr>
            <tr v-if="!client.permissions.length">
              <td colspan="3" class="font-weight-bold text-muted font-italic py-2">{{$localizer('No permissions')}}</td>
            </tr>
            <tr v-for="permission in client.permissions">
              <td class="align-middle">
                <strong>{{permission.displayName}}</strong>
                <small class="font-weight-bold text-muted font-italic">({{permission.name}})</small>
                <br><small class="text-muted font-italic">{{permission.description}}</small>
              </td>
              <td class="align-middle text-center" style="width: 74px;">
                <i class="h4" :class="{'icon-toggle-on text-success': hasPermission(permission.name, client.clientId), 'icon-toggle-off text-muted': !hasPermission(permission.name, client.clientId)}" @click="togglePermission(permission.name, client.clientId)"></i>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- =========== -->

      <!-- Danger Zone -->
      <div v-if="!isNew" class="list-group alert-danger mt-3">
        <div class="list-group-item border-danger p-0" style="overflow: hidden;">
          <h5 class="alert-danger p-2 m-0">{{$localizer('Danger Zone')}}</h5>
        </div>
        <div class="list-group-item d-flex justify-content-between align-items-start border-danger">
          <div>
            <h6 class="mb-1">{{$localizer('Delete this role')}}</h6>
            <p class="m-0">{{$localizer('NoGoingBackDelete')}}</p>
          </div>
          <button type="button" class="btn btn-outline-danger" @click="deleteRole()">{{$localizer('Delete')}}</button>
        </div>
      </div>
      <!-- =========== -->
    </form>
  </div>
</template>

<script>
module.exports = {
  data: function () {
    return {
      role: null,
      clients: null,
      permissionsFilter: ''
    };
  },
  computed: {
    isNew() {
      return this.$route.params.id === 'new';
    },
    filteredClients() {
      try {
        return JSON.parse(JSON.stringify(this.clients))
        .filter(a => JSON.stringify(a).toLowerCase().indexOf(this.permissionsFilter.toLowerCase()) !== -1)
        .map(a => {
          a.permissions = a.permissions.filter(x => JSON.stringify(x).toLowerCase().indexOf(this.permissionsFilter.toLowerCase()) !== -1);
          return a;
        });
      } catch (e) {
        return null;
      }
    }
  },
  methods: {
    save() {
      api.saveRole(this.role).then(response => {
        this.$notyf.success(this.$localizer('Saved'));
        this.$router.push(`/role/${response.id}`);
      }).catch(error => {
        this.$notyf.error(this.$localizer('Error'));
      });
    },
    deleteRole() {
      swal({
        title: this.$localizer("Delete?"),
        text: this.$localizer("This action can't be undone."),
        icon: "warning",
        buttons: true,
        dangerMode: true,
      }).then((willDelete) => {
        if (willDelete) {
          api.deleteRole(this.role.id).then(() => {
            notyf.success(this.$localizer('Role removed'));
            this.$router.push('/role');
          }).catch(error => {
            this.$notyf.error(this.$localizer('Error'));
          });
        }
      });
    },
    hasPermission(permissionName, clientId) {
      return this.role.permissions.find(x => x.name === permissionName && x.clientId === clientId);
    },
    togglePermission(permissionName, clientId) {
      const index = this.role.permissions.findIndex(x => x.name === permissionName && x.clientId === clientId);
      if (index === -1) {
        this.role.permissions.push({ name: permissionName, clientId });
      } else {
        this.role.permissions.splice(index, 1);
      }
    },
    loadData() {
      this.role = null;
      if (this.isNew) {
        this.role = {
          name: '',
          displayName: '',
          permissions: []
        };
      } else {
        api.getRole(this.$route.params.id).then(role => this.role = role);
      }
    }
  },
  watch: {
    "$route.params.id": function () {
      this.loadData();
    }
  },
  created() {
    this.loadData();
    api.getClients(true).then(clients => this.clients = clients);
  }
}
</script>
