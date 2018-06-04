export const PAGES_MENU =
	[{
		path: 'hiera',
		children: [
			{
				path: 'edit',
				data: {
					menu: {
						title: 'Hiera Edit',
						icon: 'ion-android-home',
						order: 0
					}
				}
			},
			{
				path: 'parameter/search',
				data: {
					menu: {
						title: 'Parameter Search',
						icon: 'ion-edit',
						selected: false,
						expanded: false,
						order: 100,
					}
				},
			},
			{
                path: 'Parameteredit',
				data: {
					menu: {
						title: 'Parameter Edit',
						icon: 'ion-stats-bars',
						selected: false,
						expanded: false,
						order: 200,
					}
				}
			}]
	}
];
