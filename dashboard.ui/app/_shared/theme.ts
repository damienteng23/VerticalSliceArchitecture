import { createTheme, PaletteOptions } from '@mui/material/styles';

declare module '@mui/material/styles' {
  interface Palette {
    dashboard: {
      header: string;
      sidebar: string;
      mainAreaBackground: string;
      selectedOptionBackground: string;
      darkBackground: string;
    };
  }
  interface PaletteOptions {
    dashboard?: {
      header?: string;
      sidebar?: string;
      mainAreaBackground?: string;
      selectedOptionBackground?: string;
      darkBackground?: string;
    };
  }
}

export const theme = createTheme({
  palette: {
    mode: 'light',
    dashboard: {
      header: '#738290',
      sidebar: '#A1B5D8',
      mainAreaBackground: '#FFFCF7',
      selectedOptionBackground: '#A9B5B6',
      darkBackground: '##4E4D4D',
    },
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        root: ({ theme }) => ({
          backgroundColor: theme.palette.dashboard.header,
          boxShadow: '0px 4px 12px -2px rgba(0, 0, 0, 0.5)',
          zIndex: theme.zIndex.drawer + 1,
          position: 'fixed',
        }),
      },
    },
    MuiDrawer: {
      defaultProps: {
        variant: 'temporary',
        ModalProps: {
          keepMounted: true, // better for mobile SEO/performance
        },
      },
      styleOverrides: {
        root: ({ theme }) => ({
          // Rules for the Permanent (Docked) Sidebar
          '&.MuiDrawer-docked': {
            [theme.breakpoints.down('lg')]: {
              display: 'none', // Hide permanent drawer on mobile
            },
            [theme.breakpoints.up('lg')]: {
              display: 'block', // Show permanent drawer on desktop
            },
          },
          // Rules for the Temporary (Mobile) Sidebar
          '&.MuiModal-root': {
            [theme.breakpoints.up('lg')]: {
              display: 'none', // Hide mobile drawer on desktop
            },
          }
        }),
        paper: ({ theme }) => ({
          width: 256, // drawerWidth
          boxSizing: 'border-box',
          backgroundColor: theme.palette.dashboard.sidebar,
          borderRight: `1px solid ${theme.palette.divider}`,
          boxShadow: '4px 0px 12px -2px rgba(0, 0, 0, 0.5)',
        }),
      },
    },
  },
});