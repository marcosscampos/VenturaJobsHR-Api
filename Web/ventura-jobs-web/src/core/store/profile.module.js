// action types
export const UPDATE_PERSONAL_INFO = "updateUserPersonalInfo";
export const UPDATE_ACCOUNT_INFO = "updateUserAccountInfo";

// mutation types
export const SET_PERSONAL_INFO = "setPersonalInfo";
export const SET_ACCOUNT_INFO = "setAccountInfo";

const state = {

  current_user_profile: {
    email: "sir@stranger.com",
    username: "sstranger",
    firstName: 'Stranger',
    fullName: 'Sir Stranger',
    initials: 'SS'
  }
};

if (window.blue) {
  window.blue.auth.getUserProfile().then((userProfile) => {
    state.current_user_profile = {
      ...userProfile,
      fullName: userProfile.firstName,
      firstName: userProfile.firstName.indexOf(' ') > -1 ? userProfile.firstName.split(' ')[0] : userProfile.firstName,
      initials: userProfile.lastName ? userProfile.firstName[0] + userProfile.lastName[0] : userProfile.firstName[0] + userProfile.firstName[1]
    }
  })
}

const getters = {
  currentUserProfile(state) {
    return state.current_user_profile;
  },
};


export default {
  state,
  getters
};
