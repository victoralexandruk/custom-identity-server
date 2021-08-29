<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h3>{{$localizer('User')}}</h3>
      <div class="btn-toolbar mb-2 mb-md-0">
        <!-- <button type="button" class="btn btn-sm btn-outline-secondary">Export</button> -->
      </div>
    </div>
    <div v-if="!user" class="d-flex justify-content-center">
      <div class="spinner-border text-primary" role="status">
        <span class="sr-only">Loading...</span>
      </div>
    </div>
    <div v-if="user">
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <tbody>
            <tr>
              <th class="w-25">{{$localizer('UserName')}}</th>
              <td>{{user.userName}}</td>
            </tr>
            <tr>
              <th>{{$localizer('FullName')}}</th>
              <td>{{user.fullName}}</td>
            </tr>
            <tr>
              <th>{{$localizer('Active')}}</th>
              <td>{{user.active}}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script>
module.exports = {
  data: function () {
    return {
      user: null,
      roles: [],
      editRole: null
    };
  },
  methods: {
    // loadRoles() {
    //   api.searchRoles('userId', this.$route.params.id, true).then(roles => this.roles = roles);
    // },
    // openModalRole(role) {
    //   this.editRole = role || {
    //     userId: this.id,
    //     roleId: ''
    //   };
    //   $('#modalRole').modal('show');
    // },
    // deleteClient(id) {
    //   swal({
    //     title: this.$localizer("Delete?"),
    //     text: this.$localizer("This action can't be undone."),
    //     icon: "warning",
    //     buttons: true,
    //     dangerMode: true,
    //   }).then((willDelete) => {
    //     if (willDelete) {
    //       api.deleteClient(id).then(() => {
    //         notyf.success(this.$localizer('Client removed!'));
    //       });
    //     }
    //   });
    // }
  },
  created() {
    if (this.$route.params.id === 'new') {
      this.user = {
        userName: '',
        fullName: ''
      };
    } else {
      api.getUser(this.$route.params.id).then(user => this.user = user);
    }
  }
}
</script>
