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
        }),
      },
    },
  },
});